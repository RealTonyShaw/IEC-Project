using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lazer : AbstractStrafeSkill
{
    private GameObject missilePrefab;
    private GameObject tmp;

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += Data.AccuracyCooldownSpeed * dt;
    }

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.Lazer);
        missilePrefab = Data.Prefabs[0];
        if (missilePrefab == null)
            Debug.LogError("未能找到 Lazer Prefab");
    }

    protected override Missile Shoot()
    {
        Vector3 dir = Gamef.GenerateRandomDirection(Caster.SpawnTransform.forward, Caster.RuntimeAccuracy, random);
        tmp = Gamef.Instantiate(missilePrefab, SpawnTransform.position, Quaternion.LookRotation(dir));
        Missile missile = tmp.GetComponent<Missile>();
        missile.Init(Caster, null, this);
        Caster.RuntimeAccuracy -= Data.AccuracyHeatupSpeed;
        return missile;
    }
}
