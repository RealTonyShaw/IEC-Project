using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public static AimController Instance { get; private set; }
    public long CurrentTargetInstance
    {
        get; private set;
    }
    [SerializeField]
    Unit targetForStrafeSkill = null;
    /// <summary>
    /// 连射型技能的追踪目标
    /// </summary>
    public Unit TargetForStrafeSkill
    {
        set
        {
            if (targetForStrafeSkill != value)
            {
                targetForStrafeSkill = value;
                if (GameCtrl.IsOnlineGame)
                {
                    // sync target
                    if (GameCtrl.PlayerUnit != null)
                    {
                        int id = value == null ? -1 : value.attributes.ID;
                        DataSync.SyncAimTarget(Gamef.SystemTimeInMillisecond,
                        GameCtrl.PlayerUnit.attributes.ID, id);
                    }
                }
            }
        }

        get
        {
            return targetForStrafeSkill;
        }
    }

    private Unit target = null;
    /// <summary>
    /// 点射型技能的追踪目标
    /// </summary>
    public Unit TargetForBurstfireSkill
    {
        get
        {
            return target;
        }

        private set
        {
            if (value != null && value == target)
            {
                AimingTime += Time.deltaTime;
                Debug.Log(string.Format("Aiming at {0} for {1} sec", target.gameObject.name, AimingTime));
            }
            else
            {
                target = value;
                AimingTime = 0f;
            }
        }
    }

    /// <summary>
    /// 点射型技能的已瞄准时间
    /// </summary>
    public float AimingTime
    {
        get;
        private set;
    }
    private void Awake()
    {
        Instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        Unit tmp = CameraGroupController.Instance.GetClosestUnit();
        if (InputMgr.AimingButtonPressed)
        {
            TargetForBurstfireSkill = tmp;
        }
        else
        {
            TargetForBurstfireSkill = null;
        }
        TargetForStrafeSkill = tmp;
    }
}
