﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTable
{
    /// <summary>
    /// 对玩家所控制的施法者和出生 Transform 初始化技能表
    /// </summary>
    /// <param name="caster">施法者 Unit</param>
    void Init(Unit caster);
    /// <summary>
    /// 转换到玩家所选的技能
    /// </summary>
    /// <param name="cellIndex">技能槽位序号，可以是1,2,3</param>
    void SwitchCell(int cellIndex);
    /// <summary>
    /// 获得现在玩家技能，可能是 1、2、3 号技能
    /// </summary>
    /// <returns>技能，1、2 或者 3 号技能</returns>
    ISkill CurrentSkill { get; }

    /// <summary>
    /// 获得现在玩家技能槽位，可能是 1、2、3 号技能槽位
    /// </summary>
    /// <returns>技能槽位，1、2 或者 3 号技能槽位</returns>
    ISkillCell CurrentCell { get; }

    /// <summary>
    /// 获得现在玩家技能槽位的序号，可能是 1、2、3
    /// </summary>
    int CurrentIndex { get; }
}
