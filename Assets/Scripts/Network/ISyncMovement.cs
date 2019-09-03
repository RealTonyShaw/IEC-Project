using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该接口用于进行位置同步。该接口的实例作为Unit的成员变量存储于Unit中。
/// SyncTransform和SyncAcceleration方法会在客户端接收到服务器发来的同步协议并解析后，由解析协议的方法调用。
/// Init方法用于初始化，传入并记录接受同步的单位。
/// Update方法会在Unit的Update或FixedUpdate中调用。Update方法会根据SyncTransform和SyncAcceleration记录的数据进行同步。
/// 
/// 位置同步中大同步（SyncTransform）的误差矫正和预测由两点三次埃尔米特插值实现。根据本次和上次的值可以推算出之后的可能运动轨迹。
/// 而小同步（SyncAcceleration）的误差矫正和预测由三点埃尔米特插值实现。根据本次和上两次的值可以算出之后的可能运动轨迹。
/// </summary>
public interface ISyncMovement
{
    /// <summary>
    /// 位置大同步，用于同步单位的位置、转向、速度和角速度。
    /// </summary>
    /// <param name="instant">时刻（毫秒），即单位处于该状态的时刻</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转，以四元数形式表示</param>
    /// <param name="velocity">速度</param>
    /// <param name="angularVelocity">角速度</param>
    void SyncTransform(long instant, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity);

    /// <summary>
    /// 位置小同步，用于同步单位的加速度和摄像机方向。
    /// </summary>
    /// <param name="instant">时刻，即单位处于该状态时的时刻</param>
    /// <param name="acceleration">加速度，取值为-1,0,1，分别表示加速，倒退和不变</param>
    /// <param name="angularAcceleration">角加速度，取值为-1,0,1，分别表示左转，不变和右转</param>
    /// <param name="cameraForward">摄像机正方向</param>
    void SyncAcceleration(long instant, int acceleration, int angularAcceleration, Vector3 cameraForward);

    /// <summary>
    /// 初始化同步类。
    /// </summary>
    /// <param name="unit">接受同步的单位</param>
    void Init(Unit unit);

    /// <summary>
    /// 每帧更新，进行同步。
    /// </summary>
    /// <param name="dt">一帧的时长</param>
    void Update(float dt);
}
