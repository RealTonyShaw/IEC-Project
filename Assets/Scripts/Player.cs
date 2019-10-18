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
    }

    void Cooldown()
    {
        FloatingCanvasLeft.Instance.SetCooldown(SkillTable.CurrentSkill.Data.Cooldown);
    }

    void SwitchSkill(int index)
    {
        Sprite sprite = SkillTable.CurrentSkill.Data.Icon;
        FloatingCanvasLeft.Instance.SetSkillImage(index, sprite);
        FloatingCanvasLeft.Instance.Switch2Skill(index);
    }

    public override void Death()
    {
        base.Death();
        Shield.SetActive(false);
    }

    float lastHP = 0f;
    protected override void Update()
    {
        base.Update();
        if (lastHP > attributes.SheildPoint + 1e-2f)
        {
            shieldColorController.Trigger();
        }
        lastHP = attributes.SheildPoint;
    }
}
