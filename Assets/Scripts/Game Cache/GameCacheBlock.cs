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
    private SortedList<float, CacheUnit> list = new SortedList<float, CacheUnit>();

    private struct CacheUnit
    {
        public GameObject obj;
        public float deathTime;
    }

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
        lock (list)
        {
            // 根据规则设定死亡时间
            // 1st - 4th
            if (list.Count < 4)
            {
                lifeSpan = 180f;
            }
            // 5th - 12th
            else if (list.Count < 12)
            {
                lifeSpan = 100f;
            }
            // 13th - 20th
            else if (list.Count < 20)
            {
                lifeSpan = 50f;
            }
            // 21st - 30th
            else if (list.Count < 30)
            {
                lifeSpan = 15f;
            }
            else
            {
                // 缓存已满，顶替
                UnityEngine.Object.Destroy(Pop());
                lifeSpan = 15f;
            }
            cell.deathTime = Time.time + lifeSpan;
            // 防止 key 冲突
            lock (cacheMutex)
            {
                while (list.ContainsKey(cell.deathTime))
                {
                    cell.deathTime += 1e-5f;
                }
                list.Add(cell.deathTime, cell);
            }
        }
    }

    /// <summary>
    /// 从缓存块中提取物体(但不移除)。此方法不会激活(Enable)物体。
    /// </summary>
    /// <returns>提取到的物体。若缓存为空，则返回null</returns>
    public GameObject Retrieve()
    {
        lock (list)
        {
            if (list.Count > 0)
            {
                KeyValuePair<float, CacheUnit> pair = list.First();
                return pair.Value.obj;
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
        lock (list)
        {
            if (list.Count > 0)
            {
                KeyValuePair<float, CacheUnit> pair = list.First();
                list.RemoveAt(0);
                return pair.Value.obj;
            }
        }
        return null;
    }

    /// <summary>
    /// 刷新缓存，移除过期的缓存单位
    /// </summary>
    public void Refresh()
    {
        lock (list)
        {
            KeyValuePair<float, CacheUnit> pair;
            while (list.Count > 0 && (pair = list.First()).Key < Time.time)
            {
                list.RemoveAt(0);
                UnityEngine.Object.Destroy(pair.Value.obj);
            }
        }
    }
}

