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
    int h = 0;
    int v = 0;
    Vector3 cameraForward = Vector3.forward;
    readonly object mutex = new object();
    long lastSyncAc = 0;
    public void SyncMobileControlAxes(long instant, int h, int v)
    {
        // instant 暂时没用到，以后考虑用于误差预判
        lock (mutex)
        {
            this.h = h;
            this.v = v;
            if (instant > lastSyncAc)
            {
                lastSyncAc = instant;
            }
            unit.SyncMovement.SyncAcceleration(lastSyncAc, h, v, cameraForward);
        }
    }

    public void SyncCameraFoward(long instant, Vector3 forward)
    {
        lock (mutex)
        {
            cameraForward = forward;
            if (instant > lastSyncAc)
            {
                lastSyncAc = instant;
            }
            unit.SyncMovement.SyncAcceleration(lastSyncAc, h, v, forward);
        }
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
