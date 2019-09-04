using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISyncPlayerInput
{
    /// <summary>
    /// 同步移动控制轴的值。即水平轴和垂直轴的值。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    /// <param name="h">水平轴的值 Horizontal Axis</param>
    /// <param name="v">垂直轴的值 Vertical Axis</param>
    void SyncMobileControlAxes(long instant, int h, int v);
    /// <summary>
    /// 同步切换到的技能栏的序号。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    /// <param name="skillIndex">技能栏序号</param>
    void SyncSwitchSkill(long instant, int skillIndex);
    /// <summary>
    /// 同步鼠标左键按下事件。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    void SyncMouseButton0Down(long instant);
    /// <summary>
    /// 同步鼠标左键松开事件。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    void SyncMouseButton0Up(long instant);
    /// <summary>
    /// 初始化同步类。
    /// </summary>
    /// <param name="unit">接受同步的单位</param>
    void Init(Unit unit);
}
