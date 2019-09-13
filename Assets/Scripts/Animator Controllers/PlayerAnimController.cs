using System;
using UnityEngine;
public class PlayerAnimController : AnimatorController
{
    public Mover mover;
    public Unit unit;
    [SerializeField]
    int flyMode = -1;

    private void Awake()
    {
        unit.StartCastingEvnt.AddListener(StartChannelling);
        unit.StopCastingEvnt.AddListener(StopChannelling);
    }

    private void Update()
    {
        UpdateFlyMode();
    }

    public void ProjectileAttack()
    {
        animator.SetTrigger(paramNames[5]);
    }

    public void StartChannelling()
    {
        if (unit.SkillTable.CurrentSkill.Data.SkillType != SkillType.StrafeSkill)
            return;
        //animator.SetBool(paramNames[9], true);
        animator.SetTrigger(paramNames[13]);
    }

    public void StopChannelling()
    {
        if (unit.SkillTable.CurrentSkill.Data.SkillType != SkillType.StrafeSkill)
            return;
        //animator.SetBool(paramNames[9], false);
        animator.SetTrigger(paramNames[14]);
    }

    private void UpdateFlyMode()
    {
        int h = Mathf.RoundToInt(mover.H);
        int v = Mathf.RoundToInt(mover.V);
        int res = -1;
        switch (v)
        {
            case -1:
                res = 1;
                break;
            case 1:
                res = 0;
                break;
        }
        if (res != flyMode)
        {
            if (flyMode != -1)
            {
                animator.SetBool(paramNames[flyMode], false);
            }
            flyMode = res;
            if (flyMode != -1)
            {
                animator.SetBool(paramNames[flyMode], true);
            }
        }
    }

    string[] paramNames =
    {
        "Fly Forward",  // 0
        "Fly Backward",
        "Fly Right",
        "Fly Left",
        "Melee Attack",
        "Projectile Attack", // 5
        "Shockwave Attack",
        "Cast Spell 01",
        "Cast Spell 02",
        "Channeling",
        "Defend",  // 10
        "Take Damage",
        "Die",
        "Start Channeling",
        "Stop Channeling",
    };
}

