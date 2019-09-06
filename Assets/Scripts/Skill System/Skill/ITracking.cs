
/// <summary>
/// 如果技能发出的投掷物有追踪能力，则该技能类需要实现本接口(ITracking)
/// Skill Cell 会根据技能数据中的 IsTracking 和技能是否实现 ITracking 接口判断是否需要给技能设置追踪目标。
/// </summary>
public interface ITracking
{
    /// <summary>
    /// 追踪目标
    /// </summary>
    Unit Target
    {
        get; set;
    }
}

