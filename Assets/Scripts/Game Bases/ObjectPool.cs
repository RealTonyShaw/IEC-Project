using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPool<T> : IEnumerable<T>
{
    public const int BLK_LENGTH = 1 << 8;
    public const int BLK_MASK = ~0xff;
    public const int OFFSET_MASK = 0xff;
    private class Cell
    {
        public T content;
        public bool isValid = false;
        public readonly object mutex = new object();
    }

    // 修改blkList和MaxLength需要使用该锁。
    private readonly object blkMutex = new object();
    List<Cell[]> blkList = new List<Cell[]>();
    // 最大可能的ID（实际上，最大ID应当小于MaxLength）
    public int MaxLength { get; private set; } = 0;

    // 增减idQueue需要使用该锁。
    private readonly object idQueueMutex = new object();
    //存储已经不被占用的id（即对象被清除）
    Queue<int> idQueue = new Queue<int>();
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
            lock (blkMutex)
            {
                res = MaxLength++;
                //如果超负荷，则申请一个新的数组
                if ((res & BLK_MASK) >= blkList.Count)
                {
                    ExtendPool();
                }
            }
        }

        Cell c = blkList[res & BLK_MASK][res & OFFSET_MASK];
        lock (c.mutex)
        {
            c.isValid = true;
            c.content = obj;
        }
        Debug.Log("Add " + typeof(T).ToString() + " to blk " + (res & BLK_MASK) + ", cell " + (res & OFFSET_MASK));
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
            lock (blkMutex)
            {
                prevLen = MaxLength;
                MaxLength = id + 1;
                while ((id & BLK_MASK) >= blkList.Count)
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
        Cell c = blkList[id & BLK_MASK][id & OFFSET_MASK];
        lock (c.mutex)
        {
            c.isValid = true;
            c.content = obj;
        }
        Debug.Log("Add " + typeof(T).ToString() + " to blk " + (id & BLK_MASK) + ", cell " + (id & OFFSET_MASK));
        return id;
    }

    /// <summary>
    /// 获取ID对应对象
    /// </summary>
    /// <param name="id">对象ID</param>
    /// <returns>对象</returns>
    public T GetObject(int id)
    {
        return blkList[id & BLK_MASK][id & OFFSET_MASK].content;
    }
    /// <summary>
    /// 检查ID是否被占用
    /// </summary>
    /// <param name="id">对象ID</param>
    /// <returns>是否被占用。True，表示被占用；false，表示不被占用</returns>
    public bool CheckID(int id)
    {
        return blkList[id & BLK_MASK][id & OFFSET_MASK].isValid;
    }
    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="id">对象ID</param>
    public void RemoveObject(int id)
    {
        Cell c = blkList[id & BLK_MASK][id & OFFSET_MASK];
        lock (c.mutex)
        {
            c.isValid = false;
        }
        lock (idQueueMutex)
            idQueue.Enqueue(id);
    }

    public ObjectPool()
    {
        ExtendPool();
    }

    private void ExtendPool()
    {
        Cell[] cells = new Cell[BLK_LENGTH];
        for (int i = 0; i < BLK_LENGTH; i++)
        {
            cells[i] = new Cell();
        }
        blkList.Add(cells);
    }

    public delegate void TraversalHandler(T obj);
    /// <summary>
    /// 遍历所有池中对象
    /// </summary>
    /// <param name="handler"></param>
    public void Traversal(TraversalHandler handler)
    {
        foreach (Cell[] cells in blkList)
        {
            for (int i = 0; i < BLK_LENGTH; i++)
            {
                if (cells[i].isValid)
                {
                    handler(cells[i].content);
                }
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new ObjectPoolEnumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new ObjectPoolEnumerator<T>(this);
    }
}