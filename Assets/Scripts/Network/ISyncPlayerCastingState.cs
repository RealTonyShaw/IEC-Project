using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 该接口用于进行玩家施法状态同步。该接口的实例作为Unit的成员变量存储于Unit中。
/// </summary>
public interface ISyncPlayerCastingState
{
    /// <summary>
    /// 同步技能开始施法。
    /// </summary>
    /// <param name="instant">开始施法的时刻</param>
    /// <param name="skillIndex">技能序号，指玩家的技能槽位的序号，取值1,2,3,4</param>
    void SyncStart(long instant, int skillIndex);

    /// <summary>
    /// 同步技能停止施法。
    /// </summary>
    /// <param name="instant">停止施法的时刻</param>
    /// <param name="skillIndex">技能序号，指玩家的技能槽位的序号，取值1,2,3,4</param>
    void SyncStop(long instant, int skillIndex);

    /// <summary>
    /// 初始化同步类。
    /// </summary>
    /// <param name="unit">接受同步的单位</param>
    void Init(Unit unit);
}
