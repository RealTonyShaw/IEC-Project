using System;
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
        throw new NotImplementedException();
    }

    public void SyncStop(long instant, int skillIndex)
    {
        throw new NotImplementedException();
    }
}
