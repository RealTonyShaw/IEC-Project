using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TestBurstfireSkill : AbstractBurstfireSkill
{

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += dt * Data.AccuracyCooldownSpeed;
    }

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.TestBurstfireSkill);
    }

    protected override Missile Shoot()
    {
        Debug.Log("Burstfire Accuracy : " + Caster.RuntimeAccuracy);
        this.Caster.RuntimeAccuracy -= Data.AccuracyHeatupSpeed;
        return null;
    }
}
