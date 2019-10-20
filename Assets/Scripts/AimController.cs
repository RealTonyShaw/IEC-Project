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
    Unit target = null;
    long lastAimedInstant = -1;
    /// <summary>
    /// 连射型技能的追踪目标
    /// </summary>
    public Unit Target
    {
        set
        {
            if (target != value)
            {
                target = value;
                if (GameCtrl.IsOnlineGame)
                {
                    // sync target
                    if (GameCtrl.PlayerUnit != null)
                    {
                        int id = (value == null) ? -1 : value.attributes.ID;
                        lastAimedInstant = Gamef.SystemTimeInMillisecond;
                        Debug.Log(string.Format("Send sync target ID {0} -> ID {1}", GameCtrl.PlayerUnit.attributes.ID, id));
                        DataSync.SyncAimTarget(lastAimedInstant, GameCtrl.PlayerUnit.attributes.ID, id);
                    }
                }
            }
        }

        get
        {
            return target;
        }
    }

    //private Unit target = null;
    ///// <summary>
    ///// 点射型技能的追踪目标
    ///// </summary>
    //public Unit TargetForBurstfireSkill
    //{
    //    get
    //    {
    //        return target;
    //    }

    //    private set
    //    {
    //        if (value != null && value == target)
    //        {
    //            AimingTime += Time.deltaTime;
    //            Debug.Log(string.Format("Aiming at {0} for {1} sec", target.gameObject.name, AimingTime));
    //        }
    //        else
    //        {
    //            target = value;
    //            AimingTime = 0f;
    //        }
    //    }
    //}

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

    long lastSyncTime;
    // Update is called once per frame
    void Update()
    {
        Unit tmp = CameraGroupController.Instance.GetClosestUnit();
        //if (InputMgr.AimingButtonPressed)
        //{
        //    TargetForBurstfireSkill = tmp;
        //}
        //else
        //{
        //    TargetForBurstfireSkill = null;
        //}
        Target = tmp;

        if (GameCtrl.IsOnlineGame && lastAimedInstant > 0 && GameCtrl.PlayerUnit != null)
        {
            if (Gamef.SystemTimeInMillisecond - lastSyncTime >= 100)
            {
                if (Gamef.SystemTimeInMillisecond - lastSyncTime < 120)
                    lastSyncTime += 100;
                else lastSyncTime = Gamef.SystemTimeInMillisecond;
                // sync target
                int id = (target == null || !target.attributes.isAlive) ? -1 : target.attributes.ID;
                DataSync.SyncAimTarget(lastAimedInstant, GameCtrl.PlayerUnit.attributes.ID, id);
            }
        }
    }
}
