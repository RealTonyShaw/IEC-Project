using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该接口用于进行单位状态同步。该接口的实例作为Unit的成员变量存储于Unit中。
/// 接口可用于同步单位的生命值HP和魔法值MP。
/// </summary>
public interface ISyncUnitState
{
    /// <summary>
    /// 同步单位的生命值。
    /// </summary>
    /// <param name="instant">时刻，即单位处于该生命值的时刻</param>
    /// <param name="HP">单位的生命值</param>
    void SyncHP(long instant, float HP);

    /// <summary>
    /// 同步单位的魔法值。
    /// </summary>
    /// <param name="instant">时刻，即单位处于该魔法值的时刻</param>
    /// <param name="MP">单位的魔法值</param>
    void SyncMP(long instant, float MP);

    /// <summary>
    /// 初始化同步类。
    /// </summary>
    /// <param name="unit">接受同步的单位</param>
    void Init(Unit unit);
}
