using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 这个部分用于记录与网络服务器相关的一些参数。
 * 同步间隔(synchronization interval)的单位是毫秒(ms)
 * 
*/
public partial class GameDB
{
    // 摄像机正方向同步间隔
    public const int SYNC_AC_INTERVAL = 33;
    // 位置转向同步间隔
    public const int SYNC_TRANSFORM_INTERVAL = 300;
    public const int SYNC_UNIT_STATE_INTERVAL = 100;
}