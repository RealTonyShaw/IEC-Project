using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个类是对持续型技能及其抽象父类的测试，只有log功能。
/// </summary>
public class Skill_TestContinuousSkill : AbstractContinuousSkill
{
    private GameObject missilePrefab;
    private GameObject tmp;
    protected System.Random random;

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += dt * Data.AccuracyCooldownSpeed;
    }

    public Unit Target { get; set; } = null;

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.TestContinuousSkill);
        missilePrefab = Data.Prefabs[0];
        if (missilePrefab == null)
            Debug.LogError("未能找到 Ice ball prefab");
    }

    protected override void Start()
    {
        random = new System.Random((int)(Time.time * 1000f));

        Vector3 dir = Gamef.GenerateRandomDirection(Caster.SpawnTransform.forward, Caster.RuntimeAccuracy, random);
        tmp = Gamef.Instantiate(missilePrefab, SpawnTransform.position, Quaternion.LookRotation(dir));
        Missile missile = tmp.GetComponent<Missile>();
        missile.Init(Caster, Target, this);
        //Debug.Log("Strafe Accuracy : " + Caster.RuntimeAccuracy);
        Caster.RuntimeAccuracy -= Data.AccuracyHeatupSpeed;
    }

    protected override void Stop()
    {
        Debug.Log("哦豁，您成功停止了一个持续型技能！");
    }
}


