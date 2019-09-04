using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SyncPlayerCasting : ISyncPlayerCastingState
{
    Unit unit;
    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void SyncStart(long instant, int skillIndex)
    {
        // get system time. MUST make sure that the system time would not tremble.
        long sysTime = 0;
        StartCasting cast = new StartCasting(unit, skillIndex, instant);
        // 施法事件还未发生
        if (sysTime < instant)
        {
            Gamef.DelayedExecution(cast.Start, (instant - sysTime) / 1000f);
        }
        else if (sysTime == instant)
        {
            cast.Start();
        }
        else
        {
            cast.Start();
        }
    }

    public void SyncStop(long instant, int skillIndex)
    {
        unit.SkillTable.SwitchCell(skillIndex);
        unit.SkillTable.CurrentCell.ForceToStopCasting();
    }

    private class StartCasting
    {
        readonly Unit unit;
        readonly int skillIndex;
        readonly long instant;
        public StartCasting(Unit unit, int skillIndex, long instant)
        {
            this.unit = unit;
            this.skillIndex = skillIndex;
            this.instant = instant;
        }

        public void Start()
        {
            unit.SkillTable.SwitchCell(skillIndex);
            ISkill skill = unit.SkillTable.CurrentSkill;
            // 设置施法时刻
            if (skill is ISkillCastInstant castInstant)
            {
                castInstant.SetInstant(instant);
            }
            unit.SkillTable.CurrentCell.OnMouseButtonDown();
        }
    }
}
