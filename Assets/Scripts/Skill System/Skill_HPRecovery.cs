using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触发后以 bonus 的速率恢复 SP
/// </summary>
public class Skill_HPRecovery : AbstractContinuousSkill
{
    private GameObject skillPrefab;
    public Unit Target { get; set; } = null;
    private GameObject tmp;

    public override void AccuracyCooldown(float dt)
    {
        
    }

    protected override void LoadData()
    {
        Data = Gamef.LoadSkillData(SkillName.HPRecovery);
        skillPrefab = Data.Prefabs[0];
        if (skillPrefab == null)
            Debug.LogError("未能找到魔法阵 prefab");
    }

    protected override void Start()
    {
        Debug.Log("哦豁，您成功释放了生命恢复技能！");
        // 生命恢复速度
        Caster.attributes.SPRegenerationRate.Bonus += Data.Params[0];
        tmp = Gamef.Instantiate(skillPrefab, SpawnTransform.position, new Quaternion());
    }

    protected override void Stop()
    {
        Gamef.Destroy(tmp);
        Debug.Log("哦豁，您成功停止了生命恢复技能！");
    }
}
