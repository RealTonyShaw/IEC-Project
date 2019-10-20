using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 64 个同类对象集成为 1 个 Block，每个 Pool 都由许多 Block 组成。
/// 每个对象拥有一个固定 ID，其 ID 前 6 位 bit 是其在 Block 中的偏差，前 26(32 - 6) 位是 Block 的序号。
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> : IEnumerable<T>
{
    public const int BLK_LENGTH = 0x1 << 6;
    public const int BLK_MASK = ~0x3f;
    public const int OFFSET_MASK = 0x3f;

    private readonly ulong[] OFFSET = new ulong[]
    {
        1 << 0x0, 1 << 0x1, 1 << 0x2, 1 << 0x3, 1 << 0x4, 1 << 0x5, 1 << 0x6, 1 << 0x7, 1 << 0x8, 1 << 0x9, 1 << 0xa, 1 << 0xb, 1 << 0xc, 1 << 0xd, 1 << 0xe, 1 << 0xf,
        1 << 0x10, 1 << 0x11, 1 << 0x12, 1 << 0x13, 1 << 0x14, 1 << 0x15, 1 << 0x16, 1 << 0x17, 1 << 0x18, 1 << 0x19, 1 << 0x1a, 1 << 0x1b, 1 << 0x1c, 1 << 0x1d, 1 << 0x1e, (ulong)1 << 0x1f,
        (ulong)1 << 0x20, (ulong)1 << 0x21, (ulong)1 << 0x22, (ulong)1 << 0x23, (ulong)1 << 0x24, (ulong)1 << 0x25, (ulong)1 << 0x26, (ulong)1 << 0x27, (ulong)1 << 0x28, (ulong)1 << 0x29, (ulong)1 << 0x2a, (ulong)1 << 0x2b, (ulong)1 << 0x2c, (ulong)1 << 0x2d, (ulong)1 << 0x2e, (ulong)1 << 0x2f,
        (ulong)1 << 0x30, (ulong)1 << 0x31, (ulong)1 << 0x32, (ulong)1 << 0x33, (ulong)1 << 0x34, (ulong)1 << 0x35, (ulong)1 << 0x36, (ulong)1 << 0x37, (ulong)1 << 0x38, (ulong)1 << 0x39, (ulong)1 << 0x3a, (ulong)1 << 0x3b, (ulong)1 << 0x3c, (ulong)1 << 0x3d, (ulong)1 << 0x3e, (ulong)1 << 0x3f,
    };

    private class Block
    {
        // Block 的锁
        public readonly object mutex = new object();
        // 一个格子是否被访问的快速修饰符
        public ulong validates = 0x0;
        public Cell[] cells = new Cell[BLK_LENGTH];

        public Block()
        {
            for (int i = 0; i < BLK_LENGTH; i++)
            {
                cells[i] = new Cell();
            }
        }
    }

    private class Cell
    {
        public T content;
        public bool isValid = false;
        public readonly object mutex = new object();
    }


    // 修改blkList和MaxLength需要使用该锁。
    private readonly object listMutex = new object();
    private List<Block> blks = new List<Block>();
    //private readonly object blkMutex = new object();
    //List<Cell[]> blkList = new List<Cell[]>();
    // 最大可能的ID（实际上，最大ID应当小于MaxLength）
    public int MaxLength { get; private set; } = 0;

    // 增减idQueue需要使用该锁。
    private readonly object idQueueMutex = new object();
    //存储已经不被占用的id（即对象被清除）
    Queue<int> idQueue = new Queue<int>();
    private int res;

    /// <summary>
    /// 给对象分配ID
    /// </summary>
    /// <param name="obj">目标对象</param>
    /// <returns>对象ID</returns>
    public int IDAlloc(T obj)
    {
        int res = -1;

        lock (idQueueMutex)
        {
            if (idQueue.Count > 0)
                res = idQueue.Dequeue();
        }

        if (res == -1)
        {
            lock (listMutex)
            {
                res = MaxLength++;
                //如果超负荷，则申请一个新的数组
                Debug.Log(string.Format("m id = {0}, blk {1}, cell {2}", res, (res & BLK_MASK) >> 6, res & OFFSET_MASK));
                if (((res & BLK_MASK) >> 6)>= blks.Count)
                {
                    Debug.Log("Expand");
                    ExtendPool();
                }
            }
        }

        int ofs = res & OFFSET_MASK;
        Block blk = blks[((res & BLK_MASK) >> 6)];
        lock (blk.mutex)
        {
            blk.validates = blk.validates | OFFSET[ofs];
        }
        Cell c = blk.cells[ofs];
        lock (c.mutex)
        {
            c.isValid = true;
            c.content = obj;
        }
        return res;

    }

    /// <summary>
    /// 给对象分配ID
    /// </summary>
    /// <param name="obj">目标对象</param>
    /// <param name="id">想申请的ID</param>
    /// <returns>对象ID。如果申请失败，则返回-1</returns>
    public int IDAlloc(T obj, int id)
    {
        // 
        if (id < MaxLength)
        {
            if (CheckID(id))
                return -1;
        }
        else
        {
            int prevLen;
            lock (listMutex)
            {
                prevLen = MaxLength;
                MaxLength = id + 1;
                while (((id & BLK_MASK) >> 6) >= blks.Count)
                {
                    ExtendPool();
                }
            }
            lock (idQueueMutex)
            {
                for (int i = prevLen; i < MaxLength - 1; i++)
                {
                    idQueue.Enqueue(i);
                }
            }
        }

        int ofs = id & OFFSET_MASK;
        Block blk = blks[((id & BLK_MASK) >> 6)];
        lock (blk.mutex)
        {
            blk.validates = blk.validates | OFFSET[ofs];
        }
        Cell c = blk.cells[ofs];
        lock (c.mutex)
        {
            c.isValid = true;
            c.content = obj;
        }
        return id;
    }

    /// <summary>
    /// 获取ID对应对象
    /// </summary>
    /// <param name="id">对象ID</param>
    /// <returns>对象</returns>
    public T GetObject(int id)
    {
        return id == -1 ? default : blks[(id & BLK_MASK) >> 6].cells[id & OFFSET_MASK].content;
    }
    /// <summary>
    /// 检查ID是否被占用
    /// </summary>
    /// <param name="id">对象ID</param>
    /// <returns>是否被占用。True，表示被占用；false，表示不被占用</returns>
    public bool CheckID(int id)
    {
        //return blks[id & BLK_MASK].cells[id & OFFSET_MASK].isValid;
        return (blks[(id & BLK_MASK) >> 6].validates & OFFSET[id & OFFSET_MASK]) != 0;
    }
    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="id">对象ID</param>
    public void RemoveObject(int id)
    {
        int ofs = id & OFFSET_MASK;
        Block blk = blks[(id & BLK_MASK) >> 6];
        lock (blk.mutex)
        {
            blk.validates = blk.validates ^ OFFSET[ofs];
        }
        Cell c = blk.cells[ofs];
        lock (c.mutex)
        {
            c.isValid = false;
        }
        lock (idQueueMutex)
            idQueue.Enqueue(id);
    }

    public void Clear()
    {
        lock (listMutex)
        {
            blks.Clear();
            ExtendPool();
        }
    }

    public ObjectPool()
    {
        ExtendPool();
    }

    private void ExtendPool()
    {
        blks.Add(new Block());
    }

    public delegate void TraversalHandler(T obj);
    /// <summary>
    /// 遍历所有池中对象
    /// </summary>
    /// <param name="handler"></param>
    public void Traversal(TraversalHandler handler)
    {
        foreach (Block blk in blks)
        {
            for (int i = 0; i < BLK_LENGTH; i++)
            {
                if (blk.cells[i].isValid)
                {
                    handler(blk.cells[i].content);
                }
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new ObjectPoolEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new ObjectPoolEnumerator(this);
    }

    /// <summary>
    /// Object Pool的迭代器
    /// </summary>
    public class ObjectPoolEnumerator : IEnumerator<T>
    {
        public T Current => currentID == -1 ? default : pool.GetObject(currentID);
        ObjectPool<T> pool;
        // 当前的物体在池中的ID。-1，表示当前物体为空。
        int currentID = -1;
        object IEnumerator.Current => currentID;

        public ObjectPoolEnumerator(ObjectPool<T> pool)
        {
            this.pool = pool;
            Reset();
        }

        public void Dispose()
        {
            pool = null;
        }

        public bool MoveNext()
        {
            int minID = currentID + 1;
            int maxBlkIndex = pool.blks.Count;
            Block blk;
            ulong v;
            int ofs;
            if (minID >= pool.MaxLength)
                return false;
            // try to find in the same blk
            blk = pool.blks[(minID & BLK_MASK) >> 6];
            v = blk.validates;
            ofs = minID & OFFSET_MASK;
            // if not empty
            if ((v >> ofs) != 0)
            {
                currentID = minID + RetrieveZeros(v >> ofs);
                return true;
            }

            // find from next blk
            for (int i = ((minID & BLK_MASK) >> 6) + 1; i < maxBlkIndex; i++)
            {
                blk = pool.blks[i];
                v = blk.validates;
                // 该 blk 非空的。
                if (v != 0)
                {
                    currentID = (i << 6) | RetrieveZeros(v);
                    return true;
                }
            }

            // no next item
            currentID = -1;
            return false;
        }

        int RetrieveZeros(ulong tp)
        {
            int cnt = 0;
            if ((tp & 0xffffffff) == 0) { cnt |= 32; tp >>= 32; }
            if ((tp & 0xffff) == 0) { cnt |= 16; tp >>= 16; }
            if ((tp & 0xff) == 0) { cnt |= 8; tp >>= 8; }
            if ((tp & 0xf) == 0) { cnt |= 4; tp >>= 4; }
            if ((tp & 0x3) == 0) { cnt |= 2; tp >>= 2; }
            if ((tp & 0x1) == 0) { cnt |= 1; }
            return cnt;
        }

        public void Reset()
        {
            currentID = -1;
            MoveNext();
        }
    }
}