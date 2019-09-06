using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 对于需要将给定的施法时刻作为随机参数，且需要根据施法时刻矫正误差的技能来说，
/// 其技能类需要实现该接口。
/// </summary>
public interface ISkillCastInstant
{
    /// <summary>
    /// 设置施法时刻。
    /// </summary>
    /// <param name="instant">施法时刻</param>
    void SetInstant(long instant);
}

