using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TestStrafeSkill : AbstractStrafeSkill, ITracking
{
    private GameObject missilePrefab;
    private GameObject tmp;
    int cnt = 0;
    object mutex = new object();

    public override void AccuracyCooldown(float dt)
    {
        this.Caster.RuntimeAccuracy += dt * Data.AccuracyCooldownSpeed;
    }

    public Unit Target { get; set; } = null;

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.TestStrafeSkill);
        missilePrefab = Data.Prefabs[0];
        if (missilePrefab == null)
            Debug.LogError("未能找到 Ice ball prefab");
    }

    protected override Missile Shoot()
    {
        Vector3 dir = Gamef.GenerateRandomDirection(Caster.SpawnTransform.forward, Caster.RuntimeAccuracy, random);
        tmp = Gamef.Instantiate(missilePrefab, SpawnTransform.position, Quaternion.LookRotation(dir));
        //if (GameCtrl.IsVR)
        //    Gamef.ControllerVibration(0f, 0.1f, 160, 0.5f, Valve.VR.SteamVR_Input_Sources.RightHand);
        Missile missile = tmp.GetComponent<Missile>();
        missile.Init(Caster, Target, this);
        //Debug.Log("Strafe Accuracy : " + Caster.RuntimeAccuracy);
        Caster.RuntimeAccuracy -= Data.AccuracyHeatupSpeed;
        return missile;
    }
}
