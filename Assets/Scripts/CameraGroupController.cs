using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGroupController : MonoBehaviour
{
    #region 参数
    [Header("Camera Group Controller")]
    // 用于控制位置的父物体
    public Transform PositionParent;
    public Transform RotationParent;
    // 用于控制镜头抖动的父物体
    public Transform TurbulenceParent;
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
    //public bool prescribePos = true;
    public bool lockCursor = true;
    //Quaternion xAxis = Quaternion.identity, yAxis = Quaternion.identity;
    Quaternion cameraXRot = Quaternion.identity;
    Quaternion cameraYRot = Quaternion.identity;
    Quaternion cameraZRot = Quaternion.identity;
    Quaternion targetXRot = Quaternion.identity;
    Quaternion targetYRot = Quaternion.identity;
    Quaternion targetZRot = Quaternion.identity;

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
        Vector3 pos = MoveController.Instance.EyePosition;
        PositionParent.position = pos;
    }

    private void LateUpdate()
    {
        if (GameCtrl.PlayerUnit == null)
        {
            return;
        }
        UpdatePosition();
    }

    private void FixedUpdate()
    {
        if (MoveController.Instance.PlayerMover == null)
        {
            return;
        }
        UpdateCameraRotation(Time.fixedDeltaTime);
        SetAngleAroundZAxis(Time.fixedDeltaTime);
    }

    private float xRot, yRot;
    protected virtual void UpdateCameraRotation(float dt)
    {
        xRot = Input.GetAxis("Mouse Y") * XSensitivity;
        yRot = Input.GetAxis("Mouse X") * YSensitivity;
        targetXRot *= Quaternion.Euler(-xRot, 0f, 0f);
        targetYRot *= Quaternion.Euler(0f, yRot, 0f);

        if (clampVerticalRotation)
        {
            targetXRot = ClampRotationAroundXAxis(targetXRot);
        }
        if (clampHorizontalRotation)
        {
            targetYRot = ClampRotationAroundYAxis(targetYRot);
        }

        if (smooth)
        {
            cameraXRot = Quaternion.Slerp(cameraXRot, targetXRot, dt * smoothTime);
            cameraYRot = Quaternion.Slerp(cameraYRot, targetYRot, dt * smoothTime);
        }
        else
        {
            cameraXRot = targetXRot;
            cameraYRot = targetYRot;
        }
        cameraZRot = Quaternion.Slerp(cameraZRot, targetZRot, dt * smoothTime);

        RotationParent.localEulerAngles = new Vector3(cameraXRot.eulerAngles.x, cameraYRot.eulerAngles.y, cameraZRot.eulerAngles.z);
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
    protected virtual void SetAngleAroundZAxis(float dt)
    {
        targetZRot = Quaternion.Euler(0f, 0f, MoveController.Instance.CharaLocalEulerAngles.z);
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
