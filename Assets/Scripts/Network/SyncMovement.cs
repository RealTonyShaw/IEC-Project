using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovement : ISyncMovement
{

    Unit unit;
    int recvT_Cnt = 0;
    bool recv_A = false;
    bool recv_Cam = false;

    // 保存上上次 Transform 参数
    long firstTransformInstant = 0;
    Vector3 firstPosition = Vector3.zero;
    Quaternion firstRotation = Quaternion.identity;
    Vector3 firstVelocity = Vector3.zero;
    Vector3 firstAngularVelocity = Vector3.zero;

    // 保存上次 Transform 参数
    long lastTransformInstant = 0;
    Vector3 lastPosition = Vector3.zero;
    Vector3 lastForward = Vector3.zero;
    Vector3 lastUp = Vector3.zero;
    float lastSpeed = 0;
    Vector3 lastVelocity = Vector3.zero;

    // 保存上次 Acceleration 参数
    long lastAccelerationInstant = 0;
    int lastAcceleration = 0;
    int lastAngularAcceleration = 0;

    // 保存上次 CameraForward 参数
    long lastCameraForwardInstant = 0;
    Vector3 lastCameraForward = Vector3.forward;

    // 预测的参数
    long currentInstant;
    Vector3 currentPosition;
    Quaternion currentRotation;
    Vector3 currentVelocity;
    Vector3 currentAngularVelocity;

    // 埃尔米特插值
    IHermiteInterpolation interpolate = new HermiteInterpolation();

    // Unit 的组件
    Mover mover;
    Rigidbody rb;


    public void Init(Unit unit)
    {
        this.unit = unit;
        mover = unit.GetComponent<Mover>();
        rb = unit.GetComponent<Rigidbody>();
    }

    public void SyncAcceleration(long instant, int acceleration, int angularAcceleration)
    {
        this.lastAccelerationInstant = instant;
        // 直接同步角加速度和加速度
        mover.H = angularAcceleration;
        mover.V = acceleration;
        if (!recv_A)
        {
            recv_A = true;
        }
    }

    public void SyncCameraForward(long instant, Vector3 cameraForward)
    {
        this.lastCameraForwardInstant = instant;
        this.lastCameraForward = cameraForward;
        if (!recv_Cam)
        {
            recv_Cam = true;
        }
    }

    public void SyncTransform(long instant, Vector3 position, Vector3 forward, Vector3 up, float speed)
    {
        this.lastTransformInstant = instant;
        this.lastPosition = position;
        this.lastVelocity = speed * forward;
        this.lastForward = forward;
        this.lastUp = up;
        this.lastSpeed = speed;
        // 直接同步速度
        rb.velocity = unit.transform.forward * Mathf.Lerp(rb.velocity.magnitude, lastSpeed, 0.5f);
        recvT_Cnt++;
    }

    public void Update(float dt)
    {
        if (recvT_Cnt >= 2)
        {
            // 对 unit 的三维坐标进行插值
            this.currentPosition.x = interpolate.Hermite(firstTransformInstant, firstPosition.x, firstVelocity.x, lastTransformInstant, lastPosition.x, lastVelocity.x, Gamef.SystemTimeInMillisecond);
            this.currentPosition.y = interpolate.Hermite(firstTransformInstant, firstPosition.y, firstVelocity.y, lastTransformInstant, lastPosition.y, lastVelocity.y, Gamef.SystemTimeInMillisecond);
            this.currentPosition.z = interpolate.Hermite(firstTransformInstant, firstPosition.z, firstVelocity.z, lastTransformInstant, lastPosition.z, lastVelocity.z, Gamef.SystemTimeInMillisecond);
            // 修改 unit 的位置
            unit.transform.position = Vector3.Lerp(unit.transform.position, this.currentPosition, 5f * dt);
            // rotation
            unit.transform.forward = Vector3.Slerp(this.lastForward, unit.transform.forward, 5f * dt);
            unit.transform.up = Vector3.Slerp(this.lastUp, unit.transform.up, 5f * dt);
        }

        if (recv_Cam)
        {
            // cameraForward
            mover.CameraForward = Vector3.Lerp(mover.CameraForward, this.lastCameraForward, 5f * dt);
        }
    }

}
