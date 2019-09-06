using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovement : ISyncMovement
{

    Unit unit;

    // 保存上上次 Transform 参数
    long firstTransformInstant = 0;
    Vector3 firstPosition = new Vector3();
    Quaternion firstRotation = new Quaternion();
    Vector3 firstVelocity = new Vector3();
    Vector3 firstAngularVelocity = new Vector3();

    // 保存上次 Transform 参数
    long lastTransformInstant = 0;
    Vector3 lastPosition = new Vector3();
    Vector3 lastForward = new Vector3();
    Vector3 lastUp = new Vector3();
    float lastSpeed = 0;
    Vector3 lastVelocity = new Vector3();

    // 保存上次 Acceleration 参数
    long lastAccelerationInstant = 0;
    int lastAcceleration = 0;
    int lastAngularAcceleration = 0;
    Vector3 lastCameraForward = new Vector3();

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

    public void SyncAcceleration(long instant, int acceleration, int angularAcceleration, Vector3 cameraForward)
    {
        this.lastAccelerationInstant = instant;
        this.lastCameraForward = cameraForward;
        // 直接同步角加速度和加速度
        mover.H = angularAcceleration;
        mover.V = acceleration;
        
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
    }

    public void Update(float dt)
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
        // cameraForward
        mover.CameraForward = Vector3.Lerp(mover.CameraForward, this.lastCameraForward, 5f * dt);
    }

}
