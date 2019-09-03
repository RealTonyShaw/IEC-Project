using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISkillCastInstant
{
    /// <summary>
    /// 设置施法时刻。
    /// </summary>
    /// <param name="instant">施法时刻</param>
    void SetInstant(long instant);
}

