using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SyncPlayerInput : ISyncPlayerInput
{
    Unit unit;

    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void SyncMobileControlAxes(long instant, int h, int v)
    {
        // instant 暂时没用到，以后考虑用于误差预判

    }

    public void SyncMouseButton0Down(long instant)
    {
        throw new NotImplementedException();
    }

    public void SyncMouseButton0Up(long instant)
    {
        throw new NotImplementedException();
    }

    public void SyncSwitchSkill(long instant, int skillIndex)
    {
        throw new NotImplementedException();
    }

    public void SyncVerticalAxis(long instant, int vAxis)
    {
        throw new NotImplementedException();
    }
}
