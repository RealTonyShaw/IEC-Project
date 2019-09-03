using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovement : ISyncMovement
{
    Unit unit;

    long lastTransformInstant;
    Vector3 lastPosition;
    Quaternion lastRotation;
    Vector3 lastVelocity;
    Vector3 lastAngularVelocity;

    long lastAccelerationInstant;
    int lastAcceleration;
    int lastAngularAcceleration;
    Vector3 lastCameraForward;

    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void SyncAcceleration(long instant, int acceleration, int angularAcceleration, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }
}
