using System;
using System.Collections;
using UnityEngine;

public class SyncPlayerCasting : ISyncPlayerCastingState
{
    Unit unit;
    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void SyncStart(long instant, int skillIndex)
    {
        Debug.Log("ID " + unit.attributes.ID + ": " + "try to start skill " + skillIndex + " at " + instant);
        Gamef.StartCoroutine(TryStart(instant, skillIndex));
    }

    public void SyncStop(long instant, int skillIndex)
    {
        Debug.Log("ID " + unit.attributes.ID + ": " + "try to stop skill " + skillIndex + " at " + instant);
        Gamef.StartCoroutine(TryStop(instant, skillIndex));
    }

    public void SyncTarget(long instant, Unit target)
    {
        if (unit.SkillTable.CurrentSkill is ITracking tracking)
        {
            tracking.Target = target;
            if (tracking is ITimeSensitiveTracking tsTracking)
            {
                tsTracking.SetStartAimingInstant(instant);
            }
        }
    }

    IEnumerator TryStop(long instant, int skillIndex)
    {
        long time = Gamef.SystemTimeInMillisecond;
        while (true)
        {
            if (unit.SkillTable.CurrentIndex == skillIndex && unit.SkillTable.CurrentCell.IsCasting)
            {
                Debug.Log("ID " + unit.attributes.ID + ": " + "stop skill " + skillIndex + " at " + Gamef.SystemTimeInMillisecond);
                unit.SkillTable.CurrentCell.SetInstant(instant);
                unit.SkillTable.CurrentCell.Stop();
                break;
            }
            else
            {
                if (Gamef.SystemTimeInMillisecond - time > 33)
                {
                    break;
                }
                else
                    yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator TryStart(long instant, int skillIndex)
    {
        long time = Gamef.SystemTimeInMillisecond;
        while (true)
        {
            if (unit.SkillTable.CurrentIndex == skillIndex && !unit.SkillTable.CurrentCell.IsCasting)
            {
                Debug.Log("ID " + unit.attributes.ID + ": " + "start skill " + skillIndex + " at " + Gamef.SystemTimeInMillisecond);
                // 设置施法时刻
                unit.SkillTable.CurrentCell.SetInstant(instant);
                unit.SkillTable.CurrentCell.Start();
                break;
            }
            else
            {
                if (Gamef.SystemTimeInMillisecond - time > 33)
                {
                    break;
                }
                else
                    yield return new WaitForEndOfFrame();
            }
        }
    }
}
