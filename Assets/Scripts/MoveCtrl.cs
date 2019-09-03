using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该组件用于玩家操控单位。非玩家控制单位不使用该组件。
/// </summary>
[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Rigidbody))]
public class MoveCtrl : MonoBehaviour
{
    //public GameObject CastPrefab;
    public static MoveCtrl Instance
    {
        get; set;
    }

    private Unit unit;
    private UnitAttributes unitAttributes;
    private Rigidbody rigbody;
    public Vector3 charaUp = Vector3.up;

    public Transform chara;
    public Transform eyeTransform;
    public float angularSpeed = 0f;

    private const float APPROACHING_CONST = 5f;
    private readonly float horizonConst = GameDB.HORIZONTAL_ROTATION_SPEED / GameDB.MAX_HORIZONTAL_ANGLE;
    private void Awake()
    {
        EventMgr.UnitBirthEvent.AddListener(Init);
        Instance = this;
    }

    private float v;
    private float h;
    private float angleBias = 0f;
    //private float ac;
    private void Update()
    {
        v = /*Input.GetAxis("Vertical")*/InputMgr.GetVerticalAxis();
        h = /*Input.GetAxis("Horizontal")*/InputMgr.GetHorizontalAxis();
        //ac = Input.GetKey(InputMgr.AccelerationKey) ? 1f : 0f;
    }

    private void FixedUpdate()
    {
        if (InputMgr.MobileControlKeyEnable)
        {
            Turning(Time.fixedDeltaTime);
        }
        UpdateVelocity(Time.fixedDeltaTime);

        Vector3 fwd = transform.forward;
        transform.forward = Vector3.Slerp(fwd, targetFwd, APPROACHING_CONST * Time.fixedDeltaTime);
        //speed = rigbody.velocity.magnitude;
        //angularSpeed = srb.angularVelocity.magnitude * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 目标正方向，由转向Turning设定
    /// </summary>
    Vector3 targetFwd;

    /// <summary>
    /// 速率，由Update Velocity设置
    /// </summary>
    float speed = 0f;

    /// <summary>
    /// 转向。即更新转向速度angularVelocity
    /// </summary>
    private void Turning(float dt)
    {
        if (!InputMgr.MobileControlKeyEnable) return;

        targetFwd = CameraGroupController.Instance.transform.forward;
        if (Mathf.Abs(h) > GameDB.FLOAT_ZERO)
        {
            // 更新 angle bias 的值
            angleBias += h * GameDB.HORIZONTAL_ROTATION_SPEED * dt;
        }
        //else if (Mathf.Abs(angleBias) > GameDB.FLOAT_ZERO)
        //{
        //    if (angleBias < 0f)
        //    {
        //        angleBias += GameDB.HORIZONTAL_ROTATION_SPEED * Time.fixedDeltaTime;
        //        if (angleBias > 0f)
        //        {
        //            angleBias = 0f;
        //        }
        //    }
        //    else if (angleBias > 0f)
        //    {
        //        angleBias -= GameDB.HORIZONTAL_ROTATION_SPEED * Time.fixedDeltaTime;
        //        if (angleBias < 0f)
        //        {
        //            angleBias = 0f;
        //        }
        //    }
        //}
        angleBias -= angleBias * horizonConst * Time.fixedDeltaTime;
        // 根据 angle bias 设定前行方向
        targetFwd = Quaternion.AngleAxis(angleBias, CameraGroupController.Instance.transform.up) * targetFwd;

        // 根据 angle bias 设定前行方向
        //if (h < -GameDB.FLOAT_ZERO)
        //{
        //    targetFwd = Quaternion.AngleAxis(-angleBias, CameraGroupController.Instance.transform.up) * targetFwd;
        //}
        //else if (h > GameDB.FLOAT_ZERO)
        //{
        //    targetFwd = Quaternion.AngleAxis(angleBias, CameraGroupController.Instance.transform.up) * targetFwd;
        //}
    }

    /// <summary>
    /// 加速。即更新速度Velocity, 并调整速度方向。
    /// </summary>
    /// <param name="dt">时间间隔</param>
    private void UpdateVelocity(float dt)
    {
        float v = this.v < -GameDB.FLOAT_ZERO ? this.v * GameDB.MAX_BACKWARD_SPEED_RATE : this.v;
        rigbody.velocity += transform.forward * v * unitAttributes.Acceleration * Time.fixedDeltaTime;
        // 令速度方向趋近镜头方向。
        if (Vector3.Angle(rigbody.velocity, targetFwd) < 90f + Mathf.Abs(angleBias) + GameDB.FLOAT_ZERO)
        {
            rigbody.velocity = Vector3.Lerp(rigbody.velocity.normalized, targetFwd, APPROACHING_CONST * dt).normalized * rigbody.velocity.magnitude;
        }
        else
        {
            rigbody.velocity = Vector3.Lerp(rigbody.velocity.normalized, -targetFwd, APPROACHING_CONST * dt).normalized * rigbody.velocity.magnitude;
        }
    }


    private bool isInit = false;
    private void Init(EventMgr.UnitBirthEventInfo info)
    {
        if (isInit)
            return;
        if (info.Unit.gameObject != gameObject)
            return;
        unit = info.Unit;
        unitAttributes = unit.attributes;
        rigbody = unit.rigbody;
        isInit = true;
    }
}
