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

        }
    }

    public void SyncStop(long instant, int skillIndex)
    {
        throw new NotImplementedException();
    }


    void m_startCastingImmediately()
    {
        unit.SkillTable.CurrentCell.OnMouseButtonDown();
    }

    void m_conpensate()
    {

    }
}
