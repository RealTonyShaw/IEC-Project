using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//   1） 根据存放数量递减：第1-4个，180s；第5-12个，100s；第13-20个，50s；大于20个，15s。
//   2） 存在容量上限，不超过30个同类物体。
public class GameCacheBlock
{
    // 存储缓存物体的列表。在复用时，优先复用快过期的物体。
    //private SortedList<float, CacheUnit> list = new SortedList<float, CacheUnit>();
    private readonly MySortedList list = new MySortedList();
    public readonly object BlkMutex = new object();
    private struct CacheUnit
    {
        public GameObject obj;
        public float deathTime;
    }
    readonly object listMutex = new object();
    readonly object cacheMutex = new object();
    /// <summary>
    /// 将物体存入缓存块。此方法不会取消激活(Disable)物体。
    /// </summary>
    /// <param name="gameObject"></param>
    public void Cache(GameObject gameObject)
    {
        CacheUnit cell;
        cell.obj = gameObject;
        float lifeSpan = -1e-5f;
        lock (listMutex)
        {
            int cnt = list.Count;
            // 根据规则设定死亡时间
            // 1st - 4th
            if (cnt < 4)
            {
                lifeSpan = 180f;
            }
            // 5th - 12th
            else if (cnt < 12)
            {
                lifeSpan = 100f;
            }
            // 13th - 20th
            else if (cnt < 20)
            {
                lifeSpan = 50f;
            }
            // 21st - 30th
            else if (cnt < 30)
            {
                lifeSpan = 15f;
            }
            else
            {
                // 缓存已满，顶替
                UnityEngine.Object.Destroy(list.PopFront().obj);
                lifeSpan = 15f;
            }
            cell.deathTime = Time.time + lifeSpan;
            list.Add(cell);
        }
    }

    /// <summary>
    /// 从缓存块中提取物体(但不移除)。此方法不会激活(Enable)物体。
    /// </summary>
    /// <returns>提取到的物体。若缓存为空，则返回null</returns>
    public GameObject Retrieve()
    {
        lock (listMutex)
        {
            if (list.Count > 0)
            {
                return list.Front.obj;
            }
        }
        return null;
    }

    /// <summary>
    /// 从缓存块中提取并移除物体。此方法不会激活(Enable)物体。
    /// </summary>
    /// <returns>提取到的物体。若缓存为空，则返回null</returns>
    public GameObject Pop()
    {
        lock (listMutex)
        {
            if (list.Count > 0)
            {
                return list.PopFront().obj;
            }
            return null;
        }
    }

    /// <summary>
    /// 刷新缓存，移除过期的缓存单位
    /// </summary>
    public void Refresh()
    {
        lock (listMutex)
        {
            CacheUnit cu;
            while (list.Count > 0 && (cu = list.Front).deathTime < Time.time)
            {
                list.PopFront();
                UnityEngine.Object.Destroy(cu.obj);
            }
        }
    }

    private class MySortedList
    {
        private readonly List<CacheUnit> list = new List<CacheUnit>();
        private readonly object mutex = new object();

        public void Add(CacheUnit unit)
        {
            lock (mutex)
            {
                int cnt = list.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (list[i].deathTime >= unit.deathTime)
                    {
                        list.Insert(i, unit);
                        return;
                    }
                }
                list.Add(unit);
            }
        }

        public CacheUnit Front
        {
            get
            {
                lock (mutex)
                {
                    if (list.Count > 0)
                        return list[0];
                    return default;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (mutex)
                    return list.Count;
            }
        }

        public CacheUnit PopFront()
        {
            lock (mutex)
            {
                CacheUnit res = default;
                if (list.Count > 0)
                {
                    res = list[0];
                    list.RemoveAt(0);
                }
                return res;
            }
        }

    }
}

