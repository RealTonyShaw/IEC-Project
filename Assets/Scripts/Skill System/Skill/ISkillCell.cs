using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能槽位
/// </summary>
public interface ISkillCell : ISkillCastInstant, ICastringState
{
    /// <summary>
    /// 初始化技能槽位。
    /// </summary>
    /// <param name="caster">施法者，即技能槽位的拥有者</param>
    void Init(Unit caster);
    /// <summary>
    /// 当鼠标左键被按下，开始施法。
    /// </summary>
    void Start();
    /// <summary>
    /// 当鼠标左键被松开，结束施法。
    /// </summary>
    void Stop();
    /// <summary>
    /// 当前技能槽位中的技能。
    /// </summary>
    ISkill CurrentSkill { get; set; }
}
