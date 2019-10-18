using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TestBurstfireSkill : AbstractBurstfireSkill, ITracking
{
    private GameObject missilePrefab;
    private GameObject tmp;

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += dt * Data.AccuracyCooldownSpeed;
    }

    public Unit Target { get; set; } = null;

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.TestBurstfireSkill);
        missilePrefab = Data.Prefabs[0];
        if (missilePrefab == null)
            Debug.LogError("未能找到 Ice ball prefab");
    }

    protected override Missile Shoot()
    {
        Debug.Log(string.Format("ID {0} test burst start at {1}", Caster.attributes.ID, Gamef.SystemTimeInMillisecond));
        Vector3 dir = Gamef.GenerateRandomDirection(Caster.SpawnTransform.forward, Caster.RuntimeAccuracy, random);
        tmp = Gamef.Instantiate(missilePrefab, SpawnTransform.position, Quaternion.LookRotation(dir));
        Missile missile = tmp.GetComponent<Missile>();
        missile.Init(Caster, Target, this, new MissileHitBasicHandler(), new FireBallEffectHandler());
        //Debug.Log("Strafe Accuracy : " + Caster.RuntimeAccuracy);
        Caster.RuntimeAccuracy -= Data.AccuracyHeatupSpeed;
        return missile;
    }
}
