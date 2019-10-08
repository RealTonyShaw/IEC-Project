using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触发后以 bonus 的速率提升速率
/// </summary>
public class Skill_TestContinuousSkill : AbstractContinuousSkill
{
    private GameObject missilePrefab;
    private GameObject tmp;
    private float original;
    protected System.Random random;

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += dt * Data.AccuracyCooldownSpeed;
    }

    public Unit Target { get; set; } = null;

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.TestContinuousSkill);
    }

    protected override void Start()
    {
        Debug.Log("哦豁，您成功释放了速度提升技能！");
        original = Caster.attributes.MaxV_bonus;
        Caster.attributes.MaxV_bonus += Data.Params[0];
    }

    protected override void Stop()
    {
        Caster.attributes.MaxV_bonus = original;
        Debug.Log("哦豁，您成功停止了速度提升技能！");
    }
}


