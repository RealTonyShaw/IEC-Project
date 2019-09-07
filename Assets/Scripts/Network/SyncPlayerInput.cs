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

    public void SyncMobileControlAxes(long instant, int h, int v)
    {
        unit.SyncMovement.SyncAcceleration(instant, h, v);
    }

    public void SyncCameraFoward(long instant, Vector3 forward)
    {
        unit.SyncMovement.SyncCameraForward(instant, forward);
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
