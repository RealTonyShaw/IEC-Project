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

    public void SyncHorizontalAxis(long instant, int hAxis)
    {
        throw new NotImplementedException();
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
