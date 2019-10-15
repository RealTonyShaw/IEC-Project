using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SyncPlayerInput : ISyncPlayerInput
{
    Unit unit;

    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    // 输入 h v， 输出 v h。之前写反了。
    public void SyncMobileControlAxes(long instant, int h, int v, Vector3 fwd)
    {
        unit.SyncMovement.SyncAcceleration(instant, v, h, fwd);
    }

    public void SyncMouseButton0Down(long instant)
    {
        unit.SyncPlayerCastingState.SyncStart(instant, skillIndex);
    }

    public void SyncMouseButton0Up(long instant)
    {
        unit.SyncPlayerCastingState.SyncStop(instant, skillIndex);
    }

    long lastSyncSwitch = 0;
    int skillIndex = 1;
    public void SyncSwitchSkill(long instant, int skillIndex)
    {
        if (instant > lastSyncSwitch)
        {
            lastSyncSwitch = instant;
            this.skillIndex = skillIndex;
            unit.SkillTable.SwitchCell(skillIndex);
        }
    }
}
