using UnityEngine;

public class MoveController : MonoBehaviour
{
    public static MoveController Instance
    {
        get;
        private set;
    }

    public Vector3 EyePosition => mover.EyeTransform.position;
    public Vector3 EyeEulerAngles => mover.EyeTransform.eulerAngles;
    public Vector3 CharaUp => mover.Chara.up;
    public Vector3 CharaLocalEulerAngles => mover.Chara.localEulerAngles;
    public Mover PlayerMover => mover;

    private Mover mover = null;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (mover == null && GameCtrl.PlayerUnit != null)
        {
            mover = GameCtrl.PlayerUnit.GetComponent<Mover>();
        }
        if (mover == null)
            return;
        mover.V = InputMgr.GetVerticalAxis();
        mover.H = InputMgr.GetHorizontalAxis();
        mover.CameraForward = CameraGroupController.Instance.transform.forward;
    }

    private void Update()
    {
        if (mover == null && GameCtrl.PlayerUnit != null)
        {
            mover = GameCtrl.PlayerUnit.GetComponent<Mover>();
        }
        if (mover == null)
            return;

        mover.V = InputMgr.GetVerticalAxis();
        mover.H = InputMgr.GetHorizontalAxis();
        mover.CameraForward = CameraGroupController.Instance.transform.forward;
    }
}
