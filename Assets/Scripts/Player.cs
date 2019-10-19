using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public GameObject Shield;
    public ShieldColorController shieldColorController;

    protected override void MyStart()
    {
        base.MyStart();
        if (IsLocal)
        {
            GameCtrl.PlayerUnit = this;
            FloatingCanvasLeft.Instance.SetSkillImage(1, SkillTable.CurrentSkill.Data.Icon);
            SwitchSkillEvnt.AddListener(SwitchSkill);
            StartCastingEvnt.AddListener(Cooldown);
        }
        OnHit.AddListener(triggerShield);
    }

    void Cooldown()
    {
        SkillType type = SkillTable.CurrentSkill.Data.SkillType;
        if (type == SkillType.BurstfireSkill || type == SkillType.ContinuousSkill)
            FloatingCanvasLeft.Instance.SetCooldown(SkillTable.CurrentSkill.Data.Cooldown);
    }

    void SwitchSkill(int index)
    {
        Sprite sprite = SkillTable.CurrentSkill.Data.Icon;
        FloatingCanvasLeft.Instance.SetSkillImage(index, sprite);
        FloatingCanvasLeft.Instance.Switch2Skill(index);
    }

    void triggerShield()
    {
        shieldColorController.Trigger();
    }

    public override void Death()
    {
        base.Death();
        Shield.SetActive(false);
    }

    public void SetPlayerName(string name)
    {
        if (GameCtrl.IsOnlineGame && !IsLocal)
        {
            canvas.SetName(name);
        }
    }
}
