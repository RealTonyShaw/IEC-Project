using System;
using UnityEngine;
public class PlayerAnimController : AnimatorController
{
    public Mover mover;
    public Unit unit;
    public Transform YParent;
    [SerializeField]
    int flyMode = -1;

    bool isCasting = false;
    object castStateMutex = new object();
    Vector3 fwd;

    private void Awake()
    {
        unit.StartCastingEvnt.AddListener(StartChannelling);
        unit.StartCastingEvnt.AddListener(ProjectileAttack);
        unit.StartCastingEvnt.AddListener(StartCasting);

        unit.StopCastingEvnt.AddListener(StopChannelling);
        unit.StopCastingEvnt.AddListener(StopCasting);
    }

    private void Update()
    {
        UpdateFlyMode();
    }

    private void FixedUpdate()
    {
        if (isCasting)
        {
            fwd = mover.transform.forward;
        }
        else
        {
            int h = Mathf.RoundToInt(mover.H);
            int v = Mathf.RoundToInt(mover.V);
            // 左右只能 0.5 倍速
            Vector3 dir = GameDB.MAX_HORIZON_SPEED_RATE * h * mover.transform.right;
            // 往后 0.9 倍速
            dir += (v < 0 ? -GameDB.MAX_BACKWARD_SPEED_RATE : 1) * mover.transform.forward;
            if (v < 0)
                dir = -dir;
            else if (v == 0)
                dir = mover.transform.forward;
            fwd = dir.normalized;
        }
        YParent.forward = Vector3.Slerp(YParent.forward, fwd, 8f * Time.fixedDeltaTime);
        //int v = Mathf.RoundToInt(mover.V);
        //switch (v)
        //{
        //    case -1:
        //        fwd = -fwd;
        //        break;
        //    case 0:
        //        fwd = mover.transform.forward;
        //        break;
        //    case 1:
        //        // do nothing
        //        break;
        //}

        //YParent.forward = dir.normalized;
    }

    void StartCasting()
    {
        lock (castStateMutex)
        {
            if (!isCasting)
                isCasting = true;
        }
    }

    void StopCasting()
    {
        lock (castStateMutex)
        {
            if (isCasting)
                isCasting = false;
        }
    }

    public void ProjectileAttack()
    {
        if (unit.SkillTable.CurrentSkill.Data.SkillType == SkillType.BurstfireSkill)
        {
            animator.SetTrigger(paramNames[5]);
        }
    }

    public void StartChannelling()
    {
        if (unit.SkillTable.CurrentSkill.Data.SkillType == SkillType.StrafeSkill || unit.SkillTable.CurrentSkill.Data.SkillType == SkillType.ContinuousSkill)
        {
            //animator.SetBool(paramNames[9], true);
            animator.SetBool(paramNames[9], true);
            animator.SetTrigger(paramNames[13]);
        }
    }

    public void StopChannelling()
    {
        if (unit.SkillTable.CurrentSkill.Data.SkillType == SkillType.StrafeSkill || unit.SkillTable.CurrentSkill.Data.SkillType == SkillType.ContinuousSkill)
        {
            animator.SetBool(paramNames[9], false);
            //animator.SetTrigger(paramNames[14]);
        }
    }

    private void UpdateFlyMode()
    {
        int h = Mathf.RoundToInt(mover.H);
        int v = Mathf.RoundToInt(mover.V);
        int res = -1;
        if (v == 0)
        {
            switch (h)
            {
                case -1:
                    res = 3;
                    break;
                case 1:
                    res = 2;
                    break;
            }
        }
        else
        {
            switch (v)
            {
                case -1:
                    res = 1;
                    break;
                case 1:
                    res = 0;
                    break;
            }
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

