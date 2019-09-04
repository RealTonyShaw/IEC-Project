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
    long lastTransformInstant;
    Vector3 lastPosition;
    Quaternion lastRotation;
    Vector3 lastVelocity;
    Vector3 lastAngularVelocity;

    // 保存上次 Acceleration 参数
    long lastAccelerationInstant;
    int lastAcceleration;
    int lastAngularAcceleration;
    Vector3 lastCameraForward;

    // 预测的参数
    long currentInstant;
    Vector3 currentPosition;
    Quaternion currentRotation;
    Vector3 currentVelocity;
    Vector3 currentAngularVelocity;

    // 埃尔米特插值
    IHermiteInterpolation interpolate = new HermiteInterpolation();


    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void SyncAcceleration(long instant, int acceleration, int angularAcceleration, Vector3 cameraForward)
    {
        this.lastAccelerationInstant = instant;
        this.lastAcceleration = acceleration;
        this.lastAngularAcceleration = angularAcceleration;
        this.lastCameraForward = cameraForward;
    }

    public void SyncTransform(long instant, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        this.lastTransformInstant = instant;
        this.lastPosition = position;
        this.lastRotation = rotation;
        this.lastVelocity = velocity;
        this.lastAngularVelocity = angularVelocity;
    }

    public void Update(float dt)
    {
        this.currentPosition.x = interpolate.Hermite(firstTransformInstant, firstPosition.x, firstVelocity.x, lastTransformInstant, lastPosition.x, lastVelocity.x, this.currentInstant);
        this.currentPosition.y = interpolate.Hermite(firstTransformInstant, firstPosition.y, firstVelocity.y, lastTransformInstant, lastPosition.y, lastVelocity.y, this.currentInstant);
        this.currentPosition.z = interpolate.Hermite(firstTransformInstant, firstPosition.z, firstVelocity.z, lastTransformInstant, lastPosition.z, lastVelocity.z, this.currentInstant);
        this.currentVelocity = lastVelocity;
        this.currentAngularVelocity = lastAngularVelocity;
    }

}
