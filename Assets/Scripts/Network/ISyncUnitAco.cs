using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISyncUnitAco 
{
    void UnitOnDestroy();

    /// <summary>
    /// To blind a unit.
    /// </summary>
    /// <param name="unit">The unit blinded</param>
    void init(Unit unit);
}
