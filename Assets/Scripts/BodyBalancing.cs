using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rigidbody))]
public class BodyBalancing : MonoBehaviour
{
    //空气阻尼
    public const float AIR_DAMP = 0.5f;
    public const float Z_ROT_CONST = 2f;
    //重力加速度
    public const float GRAVITY_CONST = 9.81f;
    public const float RECI_GRAVITY_CONST = 1 / GRAVITY_CONST;
    //空气阻尼与重力加速度的比值
    public const float LEAN_CONST = AIR_DAMP / GRAVITY_CONST;

    public const float APPROACHING_RATE = 5f;

    Rigidbody rb;
    private Mover mover;

    void Awake()
    {
        mover = GetComponent<Mover>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rot = transform.eulerAngles;
    }

    float sinx, cosx, x, tanf, z;
    Vector3 rot;
    Vector3 prevRot;

    void FixedUpdate()
    {
        prevRot = rot;
        rot = transform.eulerAngles;
        Vector3 av = rot - prevRot;
        av += new Vector3(av.x > 180f ? -360f : (av.x < -180f ? 360f : 0f), av.y > 180f ? -360f : (av.y < -180f ? 360f : 0f), av.z > 180f ? -360f : (av.z < -180f ? 360f : 0f));
        av *= Mathf.Deg2Rad;


        // Z轴平衡 (绕自转轴的平衡)
        // omega * v / g
        z = -Z_ROT_CONST * av.y * Vector3.Dot(rb.velocity, transform.forward) * RECI_GRAVITY_CONST;
        z = Mathf.Atan(z) * Mathf.Rad2Deg;

        // X轴平衡 (俯仰平衡)
        //Vector3 projection = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        //x = Vector3.SignedAngle(transform.forward, projection, transform.right) * Mathf.Deg2Rad;
        ////x = srb.XRot.eulerAngles.x * Mathf.Deg2Rad;
        //sinx = Mathf.Sin(x);
        //cosx = Mathf.Cos(x);
        //tanf = (LEAN_CONST * rb.velocity.magnitude - sinx) / cosx;
        //x = Mathf.Atan(tanf) * Mathf.Rad2Deg;
        x = 0f;

        mover.Chara.localRotation = Quaternion.Slerp(mover.Chara.localRotation, Quaternion.Euler(-x, 0, z), APPROACHING_RATE * Time.fixedDeltaTime);

    }
}
