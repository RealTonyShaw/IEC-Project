using UnityEngine;
//using Valve.VR;

public class MoveController : MonoBehaviour
{
    public static MoveController Instance
    {
        get;
        private set;
    }

    //public SteamVR_Input_Sources handType;
    //public SteamVR_Action_Vector2 PadPos;
    //public SteamVR_Action_Boolean pressPad;

    public Vector3 EyePosition => mover.EyeTransform.position;
    public Vector3 EyeEulerAngles => mover.EyeTransform.eulerAngles;
    public Vector3 CharaUp => mover.Chara.up;
    public Vector3 CharaLocalEulerAngles => mover.Chara.localEulerAngles;
    public Mover PlayerMover => mover;

    private Rigidbody rb;
    private Mover mover = null;
    private void Awake()
    {
        Instance = this;
    }

    void UpdateMover(Unit player)
    {
        mover = player.GetComponent<Mover>();
        rb = mover.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameCtrl.PlayerUnitChangeEvent.AddListener(UpdateMover);
        if ((mover == null && GameCtrl.PlayerUnit != null))
        {
            mover = GameCtrl.PlayerUnit.GetComponent<Mover>();
            rb = mover.GetComponent<Rigidbody>();
        }
        if (mover == null)
            return;

        mover.V = InputMgr.GetVerticalAxis();
        mover.H = InputMgr.GetHorizontalAxis();
        mover.CameraForward = CameraGroupController.Instance.transform.forward;

        if (GameCtrl.IsOnlineGame)
        {
            lastSyncA = Gamef.SystemTimeInMillisecond;
            lastSyncT = Gamef.SystemTimeInMillisecond;
            Unit unit = GameCtrl.PlayerUnit;
            long instant = Gamef.SystemTimeInMillisecond;
            DataSync.SyncMobileControlAxes(unit, instant, Mathf.RoundToInt(mover.H), Mathf.RoundToInt(mover.V), CameraGroupController.Instance.transform.forward);
            DataSync.SyncTransform(unit, instant, unit.transform.position, unit.transform.rotation, rb.velocity.magnitude);
        }
    }
    [SerializeField]
    long instant;
    [SerializeField]
    long lastSyncA;
    [SerializeField]
    long lastSyncT;
    private void Update()
    {
        if (mover == null)
            return;

        float h, v;
        Vector3 camFwd;
        //if (GameCtrl.IsVR)
        //{
        //    if (GetPressPad())
        //    {
        //        Vector2 pos = PadPos.GetAxis(handType);
        //        h = Mathf.Clamp(pos.x * 5f, -1f, 1f);
        //        v = Mathf.Clamp(pos.y * 5f, -1f, 1f);
        //    }
        //    else
        //    {
        //        h = 0f;
        //        v = 0f;
        //    }
        //    camFwd = CameraGroupController.Instance.transform.forward;
        //}
        //else
        //{
        if (GameCtrl.CursorOnGUI)
        {
            h = 0;
            v = 0;
        }
        else
        {
            h = InputMgr.GetHorizontalAxis();
            v = InputMgr.GetVerticalAxis();
        }
        camFwd = CameraGroupController.Instance.transform.forward;
        //}
        mover.V = v;
        mover.H = h;
        mover.CameraForward = camFwd;
        if (GameCtrl.IsOnlineGame)
        {
            Unit unit = GameCtrl.PlayerUnit;
            instant = Gamef.SystemTimeInMillisecond;
            if (instant - lastSyncA >= GameDB.SYNC_AC_INTERVAL)
            {
                if (instant - lastSyncA <= GameDB.SYNC_AC_INTERVAL + 8)
                {
                    lastSyncA += GameDB.SYNC_AC_INTERVAL;
                }
                else
                {
                    lastSyncA = instant;
                }
                DataSync.SyncMobileControlAxes(unit, instant, Mathf.RoundToInt(h), Mathf.RoundToInt(v), CameraGroupController.Instance.transform.forward);
            }
            if (instant - lastSyncT >= GameDB.SYNC_TRANSFORM_INTERVAL)
            {
                if (instant - lastSyncT <= GameDB.SYNC_TRANSFORM_INTERVAL + 30)
                {
                    lastSyncT += GameDB.SYNC_TRANSFORM_INTERVAL;
                }
                else
                {
                    lastSyncT = instant;
                }
                DataSync.SyncTransform(unit, instant, unit.transform.position, unit.transform.rotation, rb.velocity.magnitude);
            }
        }
    }

    //bool GetPressPad()
    //{
    //    return pressPad.GetState(handType);
    //}
}
