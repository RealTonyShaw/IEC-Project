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

    private Mover mover;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        Instance = this;
    }

    private void Start()
    {
        mover.V = InputMgr.GetVerticalAxis();
        mover.H = InputMgr.GetHorizontalAxis();
        mover.CameraForward = CameraGroupController.Instance.transform.forward;
    }

    private void Update()
    {
        mover.V = InputMgr.GetVerticalAxis();
        mover.H = InputMgr.GetHorizontalAxis();
        mover.CameraForward = CameraGroupController.Instance.transform.forward;
    }
}
