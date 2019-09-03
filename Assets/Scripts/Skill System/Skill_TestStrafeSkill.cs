using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TestStrafeSkill : AbstractStrafeSkill
{
    private GameObject fireballPrefab;
    private GameObject tmp;

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += dt * Data.AccuracyCooldownSpeed;
    }

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.TestStrafeSkill);
        fireballPrefab = Resources.Load<GameObject>("Fireball");
        if (fireballPrefab == null)
            Debug.LogError("未能找到Prefab名为：Fireball");
    }

    protected override Missile Shoot()
    {
        Vector3 dir = Gamef.GenerateRandomDirection(Caster.SpawnTransform.forward, Caster.RuntimeAccuracy, random);
        tmp = Gamef.Instantiate(fireballPrefab, SpawnTransform.position, Quaternion.LookRotation(dir));
        Missile missile = tmp.GetComponent<Missile>();
        missile.Init(Caster, AimController.Instance.TargetForStrafeSkill, this);
        Debug.Log("Strafe Accuracy : " + Caster.RuntimeAccuracy);
        Caster.RuntimeAccuracy -= Data.AccuracyHeatupSpeed;
        return missile;
    }
}
