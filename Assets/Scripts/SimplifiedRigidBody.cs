using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// 简化刚体，Simplified Rigid Body
/// </summary>
public class SimplifiedRigidBody : MonoBehaviour
{
    /// <summary>
    /// 刚体速度
    /// </summary>
    private Vector3 _velocity;
    public Vector3 velocity
    {
        get
        {
            return _velocity = rb.velocity;
        }
        set
        {
            _velocity = value;
            rb.velocity = value;
        }
    }
    /// <summary>
    /// 刚体角速度
    /// </summary>
    public Vector3 angularVelocity;
    /// <summary>
    /// 刚体质量
    /// </summary>
    public float mass = 1f;
    /// <summary>
    /// 刚体惯量矩阵（未启用）
    /// </summary>
    public float[,] MIM = new float[3, 3];
    /// <summary>
    /// 刚体绕体坐标系x、y、z轴转动的转动惯量
    /// </summary>
    public Vector3 MI = Vector3.one;

    public Transform targetTransform;

    public Quaternion XRot => xRot;

    public Quaternion YRot => yRot;

    public Quaternion ZRot => zRot;

    private bool isInit = false;

    private Rigidbody rb;

    public bool castVelocity2Rigidbody = true;
    Quaternion xRot, yRot, zRot;

    private List<Vector3> accelerations = new List<Vector3>();
    private List<Vector3> angularAccelerations = new List<Vector3>();

    #region interface
    /// <summary>
    /// 给刚体添加力
    /// </summary>
    /// <param name="force">力</param>
    /// <param name="forceMode">力的类型</param>
    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        mass = mass <= 0 ? 1e-3f : mass;
        switch (forceMode)
        {
            case ForceMode.Acceleration:
                accelerations.Add(force);
                break;
            case ForceMode.Force:
                accelerations.Add(force / mass);
                break;
            case ForceMode.VelocityChange:
                _velocity += force;
                break;
            case ForceMode.Impulse:
                _velocity += force / mass;
                break;
            default:
                Debug.Log("No Force Mode is : " + forceMode);
                break;
        }
    }

    /// <summary>
    /// 给刚体增加力矩
    /// </summary>
    /// <param name="torque">力矩</param>
    /// <param name="torqueMode">力矩类型</param>
    public void AddTorque(Vector3 torque, ForceMode torqueMode)
    {
        MI.x = MI.x <= 0f ? 1e-3f : MI.x;
        MI.y = MI.y <= 0f ? 1e-3f : MI.y;
        MI.z = MI.z <= 0f ? 1e-3f : MI.z;

        switch (torqueMode)
        {
            case ForceMode.Acceleration:
                angularAccelerations.Add(torque);
                break;
            case ForceMode.Force:
                torque.x /= MI.x;
                torque.y /= MI.y;
                torque.z /= MI.z;
                angularAccelerations.Add(torque);
                break;
            case ForceMode.VelocityChange:
                angularVelocity += torque;
                break;
            case ForceMode.Impulse:
                torque.x /= MI.x;
                torque.y /= MI.y;
                torque.z /= MI.z;
                angularVelocity += torque;
                break;
            default:
                Debug.Log("No Force Mode is : " + torqueMode);
                break;
        }
    }

    public enum ForceMode
    {
        Acceleration,
        Force,
        VelocityChange,
        Impulse,
    }
    #endregion

    private void UpdateVelocity()
    {
        if (castVelocity2Rigidbody)
            _velocity = rb.velocity;
        foreach (Vector3 a in accelerations)
        {
            _velocity += a * Time.fixedDeltaTime;
        }
        accelerations.Clear();
        if (castVelocity2Rigidbody)
            rb.velocity = _velocity;

        foreach (Vector3 b in angularAccelerations)
        {
            angularVelocity += b * Time.fixedDeltaTime;
        }
        angularAccelerations.Clear();
    }

    private void UpdateTransform()
    {
        if (!castVelocity2Rigidbody)
            transform.localPosition += _velocity * Time.fixedDeltaTime;

        Vector3 angularVelocity = this.angularVelocity * Mathf.Rad2Deg;
        yRot*= Quaternion.Euler(0f, angularVelocity.y * Time.fixedDeltaTime, 0f);
        xRot *= Quaternion.Euler(angularVelocity.x * Time.fixedDeltaTime, 0f, 0f);
        zRot *= Quaternion.Euler(0f, 0f, angularVelocity.z * Time.fixedDeltaTime);
        targetTransform.rotation = Quaternion.Euler(xRot.eulerAngles.x, yRot.eulerAngles.y, zRot.eulerAngles.z);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //如果未设置SRB目标，默认当前挂载的物体为目标
        if (targetTransform == null)
            targetTransform = transform;
        rb.useGravity = false;
        rb.freezeRotation = true;

        isInit = true;
    }

    private void FixedUpdate()
    {
        if (!isInit) return;

        UpdateVelocity();
        UpdateTransform();
    }
}
