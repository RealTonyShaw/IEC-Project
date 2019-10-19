using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSystem : MonoBehaviour
{
    public const float PRECAST_RATE = 0.2f;

    /// <summary>
    /// 需要被追踪的目标（只能是 Unit）
    /// </summary>
    public Unit enemy;

    public float trackingAbility = 0f;

    /// <summary>
    /// 最远追踪距离（目标与 Missile 距离）
    /// </summary>
    public float focusDistance = 30;
    float bf;

    /// <summary>
    /// 目标的 Transform
    /// </summary>
    public Transform target;


    bool isFollowingTarget = true;
    string targetTag;
    ///// <summary>
    ///// 是否一直面向目标
    ///// </summary>
    //bool faceTarget;
    Vector3 tempVector;

    private void Start()
    {
        bf = 0.5f / focusDistance;
    }

    public bool IsTracking { get; private set; } = false;
    public void StartTracking(Unit targetEnemy, float trackingAbility)
    {
        enemy = targetEnemy;
        if (enemy != null)
        {
            target = targetEnemy.transform;
            lastPos = target.position;
            lastPosInstant = Gamef.SystemTimeInMillisecond;
        }
        this.trackingAbility = trackingAbility;
        IsTracking = true;
        Debug.Log(string.Format("Missile {0} starts to track Unit {1}", gameObject.name, enemy.gameObject.name));
    }

    public void StopTracking()
    {
        IsTracking = false;
    }
    Vector3 lastPos;
    long lastPosInstant;
    private void FixedUpdate()
    {
        if (!IsTracking)
        {
            return;
        }
        if (enemy == null)
        {
            StopTracking();
            return;
        }
        float dis = Vector3.Distance(target.position, transform.position);
        float fix;
        // fix distance
        if (dis < focusDistance)
        {
            fix = dis * bf;
        }
        else
        {
            fix = 1f;
        }
        float tmp = trackingAbility * fix;
        Vector3 pPos = target.position;
        if (Gamef.SystemTimeInMillisecond - lastPosInstant < 30)
        {
            pPos += (target.position - lastPos) * tmp * PRECAST_RATE;
        }
        lastPos = target.position;
        lastPosInstant = Gamef.SystemTimeInMillisecond;

        Vector3 dir = pPos - transform.position;



        if (Vector3.Angle(transform.forward, dir) < 90f)
        {
            // 转向目标
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), tmp * Time.fixedDeltaTime);
        }
        

        //if (faceTarget)
        //{
        //    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0F);

        //    //MoveForward(Time.deltaTime);

        //    if (isFollowingTarget)
        //    {
        //        transform.rotation = Quaternion.LookRotation(newDirection);
        //    }
        //}

        //else
        //{
        //if (isFollowingTarget)
        //{
        //    tempVector = targetDirection.normalized;

        //    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        //}
        //else
        //{
        //    transform.Translate(tempVector * Time.deltaTime * speed, Space.World);
        //}
        //}
    }

    //private void MoveForward(float rate)
    //{
    //    transform.Translate(Vector3.forward * rate * speed, Space.Self);
    //}
}
