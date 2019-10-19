using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该组件用于玩家操控单位。非玩家控制单位不使用该组件。
/// </summary>
[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    //public GameObject CastPrefab;
    public static Mover Instance
    {
        get; set;
    }

    private Unit unit;
    private UnitAttributes unitAttributes;
    private Rigidbody rigbody;
    [SerializeField]
    private Vector3 charaUp = Vector3.up;
    [SerializeField]
    private Transform chara;
    public Transform Chara => chara;
    [SerializeField]
    private Transform eyeTransform;
    public Transform EyeTransform => eyeTransform;

    private const float APPROACHING_CONST = 5f;
    private readonly float horizonConst = GameDB.HORIZONTAL_ROTATION_SPEED / GameDB.MAX_HORIZONTAL_ANGLE;
    private void Awake()
    {
        EventMgr.UnitBirthEvent.AddListener(Init);
        Instance = this;
    }

    public float V;
    public float H;
    public Vector3 CameraForward;

    private float angleBias = 0f;

    public Vector3 TargetVelocityDir { get; private set; }
    private float reducedRate = 1f;
    // 根号下 1 / 2，用在计算速度上。
    const float param = 0.707106781f;
    private void Update()
    {
        if (!unit.attributes.isAlive)
            return;
        int h = Mathf.RoundToInt(H);
        int v = Mathf.RoundToInt(V);
        // 左右只能 0.5 倍速
        Vector3 dir = GameDB.MAX_HORIZON_SPEED_RATE * h * transform.right;
        // 往后 0.9 倍速
        dir += (v < 0 ? -GameDB.MAX_BACKWARD_SPEED_RATE : v) * transform.forward;
        // 方均根
        reducedRate = dir.magnitude * param;
        TargetVelocityDir = dir.normalized;
    }

    private void FixedUpdate()
    {
        if (!unit.attributes.isAlive)
            return;
        UpdateVelocity(Time.fixedDeltaTime);

        transform.forward = Vector3.Slerp(transform.forward, CameraForward, APPROACHING_CONST * Time.fixedDeltaTime);
        //speed = rigbody.velocity.magnitude;
        //angularSpeed = srb.angularVelocity.magnitude * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 目标正方向，由转向Turning设定
    /// </summary>
    Vector3 targetFwd;
    /// <summary>
    /// 转向。即更新转向速度angularVelocity
    /// </summary>
    private void Turning(float dt)
    {
        if (!InputMgr.MobileControlKeyEnable) return;

        targetFwd = CameraForward;
        if (Mathf.Abs(H) > GameDB.FLOAT_ZERO)
        {
            // 更新 angle bias 的值
            angleBias += H * GameDB.HORIZONTAL_ROTATION_SPEED * dt;
        }
        angleBias -= angleBias * horizonConst * Time.fixedDeltaTime;
        // 根据 angle bias 设定前行方向
        targetFwd = Quaternion.AngleAxis(angleBias, CameraGroupController.Instance.transform.up) * targetFwd;
    }

    /// <summary>
    /// 加速。即更新速度Velocity, 并调整速度方向。
    /// </summary>
    /// <param name="dt">时间间隔</param>
    private void UpdateVelocity(float dt)
    {
        rigbody.velocity += TargetVelocityDir * reducedRate * unitAttributes.Acceleration * dt;

        //float v = this.V < -GameDB.FLOAT_ZERO ? this.V * GameDB.MAX_BACKWARD_SPEED_RATE : this.V;
        //rigbody.velocity += transform.forward * v * unitAttributes.Acceleration * Time.fixedDeltaTime;
        //// 令速度方向趋近镜头方向。
        //if (Vector3.Angle(rigbody.velocity, targetFwd) < 90f + Mathf.Abs(angleBias) + GameDB.FLOAT_ZERO)
        //{
        //    rigbody.velocity = Vector3.Lerp(rigbody.velocity.normalized, targetFwd, APPROACHING_CONST * dt).normalized * rigbody.velocity.magnitude;
        //}
        //else
        //{
        //    rigbody.velocity = Vector3.Lerp(rigbody.velocity.normalized, -targetFwd, APPROACHING_CONST * dt).normalized * rigbody.velocity.magnitude;
        //}
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
