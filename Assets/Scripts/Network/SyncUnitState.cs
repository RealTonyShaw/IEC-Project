using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SyncUnitState : ISyncUnitState
{
    Unit unit;
    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void SyncHP(long instant, float HP)
    {
        // get sys time
        long sysTime = Gamef.SystemTimeInMillisecond;
        // 预判矫正
        unit.attributes.SheildPoint = HP + unit.attributes.SPRegenerationRate.Value * (instant - sysTime) / 1000f;
    }

    public void SyncMP(long instant, float MP)
    {
        // get sys time
        long sysTime = Gamef.SystemTimeInMillisecond;
        // 预判矫正
        unit.attributes.ManaPoint.Value = MP + unit.attributes.MPRegenerationRate.Value * (instant - sysTime) / 1000f;
    }
}
