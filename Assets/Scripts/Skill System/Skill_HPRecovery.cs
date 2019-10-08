using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触发后以 bonus 的速率恢复 SP
/// </summary>
public class Skill_HPRecovery : AbstractContinuousSkill
{
    public override void AccuracyCooldown(float dt)
    {
        
    }

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.HPRecovery);
    }

    protected override void Start()
    {
        Debug.Log("哦豁，您成功释放了生命恢复技能！");
        Caster.attributes.SPRegenerationRate.Bonus += Data.Params[0];
    }

    protected override void Stop()
    {
        Debug.Log("哦豁，您成功停止了生命恢复技能！");
    }
}
