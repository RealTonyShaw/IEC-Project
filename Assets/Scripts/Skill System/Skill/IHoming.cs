using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IHoming
{
    /// <summary>
    /// 设定追踪目标。
    /// </summary>
    /// <param name="target">追踪目标</param>
    void SetTarget(Unit target);
}

