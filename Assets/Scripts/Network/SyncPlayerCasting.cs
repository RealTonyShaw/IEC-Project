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
    int skillIndex;
    public void SyncStart(long instant, int skillIndex)
    {
        // get system time. MUST make sure that the system time would not tremble.
        long sysTime = 0;
        this.skillIndex = skillIndex;
        // 施法事件还未发生
        if (sysTime < instant)
        {
            Gamef.DelayedExecution(m_startCastingImmediately, (instant - sysTime) / 1000f);
        }
        else if (sysTime == instant)
        {
            m_startCastingImmediately();
        }
        else
        {
            m_conpensate(instant);
        }
    }

    public void SyncStop(long instant, int skillIndex)
    {
        unit.SkillTable.SwitchCell(skillIndex);
        unit.SkillTable.CurrentCell.ForceToStopCasting();
    }


    void m_startCastingImmediately()
    {
        unit.SkillTable.SwitchCell(skillIndex);
        unit.SkillTable.CurrentCell.OnMouseButtonDown();
    }

    void m_conpensate(long instant)
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
