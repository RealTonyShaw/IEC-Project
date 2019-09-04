using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGroupController : MonoBehaviour
{
    #region 参数
    [Header("Camera Group Controller")]
    // 用于控制位置的父物体
    public Transform PositionParent;
    // 用于控制镜头抖动的父物体
    public Transform TurbulenceParent;
    public Transform CameraGroupXAxis;
    public Transform CameraGroupYAxis;
    // 主摄像机
    public Camera MainCamera;
    // 辅助摄像机
    public List<Camera> AssistantCameras;
    [Header("Player View Control")]
    public float XSensitivity = 2f;
    public float YSensitivity = 4f;
    public bool clampVerticalRotation = true;
    public bool clampHorizontalRotation = true;
    public float MinX = -90f;
    public float MaxX = 90f;
    public float MinY = -90f;
    public float MaxY = 90f;
    public bool smooth = true;
    public float smoothTime = 15f;
    public bool lockCursor = true;
    [SerializeField]
    Transform cameraX;
    Transform cameraY;
    Quaternion cameraTargetYRot;
    Quaternion cameraTargatXRot;

    [Header("牵连效果")]
    public bool enableImplicatedEffect = true;
    public AnimationCurve FovCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1.3f);
    public float MaxVelocity = 50f;
    public float smoothTimeForFov = 3f;
    
    #endregion

    public static CameraGroupController Instance
    {
        get; private set;
    }
    
    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        //辅助计算x轴和y轴旋转
        cameraX = CameraGroupXAxis;
        cameraY = CameraGroupYAxis;
        cameraTargatXRot = cameraX.localRotation;
        cameraTargetYRot = cameraY.localRotation;

        lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (GameCtrl.PlayerUnit == null)
        {
            return;
        }
        CheckInput();
    }

    public void UpdatePosition()
    {
        prevPos = PositionParent.position;
    }

    Vector3 prevPos;

    private void FixedUpdate()
    {
        if (GameCtrl.PlayerUnit == null)
        {
            return;
        }
        PositionParent.position = MoveController.Instance.EyePosition;
        UpdatePosition();
        UpdateCameraRotation(Time.fixedDeltaTime);
        SetAngleAroundZAxis(Time.fixedDeltaTime);
    }

    private float xRot, yRot;
    private void UpdateCameraRotation(float dt)
    {
        xRot = Input.GetAxis("Mouse Y") * XSensitivity;
        yRot = Input.GetAxis("Mouse X") * YSensitivity;
        cameraTargatXRot *= Quaternion.Euler(-xRot, 0f, 0f);
        cameraTargetYRot *= Quaternion.Euler(0f, yRot, 0f);
        

        if (clampVerticalRotation)
        {
            cameraTargatXRot = ClampRotationAroundXAxis(cameraTargatXRot);
        }
        if (clampHorizontalRotation)
        {
            cameraTargetYRot = ClampRotationAroundYAxis(cameraTargetYRot);
        }

        if (smooth)
        {
            cameraX.localRotation = Quaternion.Slerp(cameraX.localRotation, cameraTargatXRot, dt * smoothTime);
            cameraY.localRotation = Quaternion.Slerp(cameraY.localRotation, cameraTargetYRot, dt * smoothTime);
        }
        else
        {
            cameraX.localRotation = cameraTargatXRot;
            cameraY.localRotation = cameraTargetYRot;
        }
        PositionParent.localEulerAngles = new Vector3(cameraX.localEulerAngles.x, cameraY.localEulerAngles.y, 0f);
        //SetAngleAroundZAxis(GameCtrl.PlayerUnit.EyeTransform.eulerAngles.z);
        //parent.rotation = GameCtrl.PlayerUnit.EyeTransform.rotation;
    }

    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && lockCursor)
        {
            lockCursor = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonUp(0) && !lockCursor)
        {
            lockCursor = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinX, MaxX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    private Quaternion ClampRotationAroundYAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleY = 2f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        float bias = angleY - MoveController.Instance.EyeEulerAngles.y;
        if (bias > 180f) bias -= 360f;
        else if (bias < -180f) bias += 360f;

        angleY = Mathf.Clamp(bias, MinY, MaxY) + MoveController.Instance.EyeEulerAngles.y;

        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        return q;
    }

    /// <summary>
    /// 设置镜头绕z轴的旋转角度。
    /// </summary>
    /// <param name="angle">角度</param>
    private void SetAngleAroundZAxis(float dt)
    {
        float angle = MoveController.Instance.CharaLocalEulerAngles.z;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, angle), 10f * dt);
    }

    /// <summary>
    /// 获得离摄像机最近的单位。最近指单位在镜头上的2D位置离镜头中央最近，即“看起来”最近。
    /// </summary>
    /// <returns>最近的单位</returns>
    public Unit GetClosestUnit()
    {
        Unit player = GameCtrl.PlayerUnit;
        if (player == null)
            return null;

        IEnumerator<Unit> it = Gamef.GetUnitEnumerator();
        Unit resUnit = null;
        float minAngle = GameDB.MAX_AIMING_ANGLE;
        Vector3 fwd = transform.forward;
        Vector3 pos = transform.position;
        Unit tmp;
        do
        {
            tmp = it.Current;
            if (tmp != player)
            {
                if (tmp == null)
                {
                    Debug.Log("???");
                    it.Reset();
                    tmp = it.Current;
                }
                float angle = Vector3.Angle(fwd, tmp.transform.position - pos);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    resUnit = tmp;
                }
            }
        } while (it.MoveNext());
        //Debug.Log("Unit Closest to Player is " + (resUnit == null ? "null" : resUnit.gameObject.name));
        return resUnit;
    }

}
