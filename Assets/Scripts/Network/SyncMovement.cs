using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovement : ISyncMovement
{

    Unit unit;

    // 保存上上次 Transform 参数
    long firstTransformInstant;
    Vector3 firstPosition;
    Quaternion firstRotation;
    Vector3 firstVelocity;
    Vector3 firstAngularVelocity;

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
        
    }

}
