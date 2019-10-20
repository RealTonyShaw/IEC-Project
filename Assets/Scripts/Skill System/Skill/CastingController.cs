using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Valve.VR;


/// <summary>
/// 施法控制器。用于将玩家的硬件输入转化为对SkillTable的指令。
/// </summary>
public class CastingController : MonoBehaviour
{
    Unit player = null;
    ISkillTable skillTable;
    int index = 1;
    //public SteamVR_Input_Sources triggerHandType = SteamVR_Input_Sources.RightHand;
    //public SteamVR_Action_Boolean trigger;
    //public SteamVR_Input_Sources padPosHandType = SteamVR_Input_Sources.LeftHand;
    //public SteamVR_Action_Vector2 padPos;
    //public SteamVR_Input_Sources pressPadHandType = SteamVR_Input_Sources.LeftHand;
    //public SteamVR_Action_Boolean pressPad;
    bool expectedCasting = false;
    readonly object castingMutex = new object();

    private void UpdatePlayer(Unit player)
    {
        this.player = player;
        skillTable = player.SkillTable;
    }

    //bool GetTriggerDown()
    //{
    //    return trigger.GetStateDown(triggerHandType);
    //}

    //bool GetTriggerUp()
    //{
    //    return trigger.GetStateUp(triggerHandType);
    //}

    //Vector2 GetPadPos()
    //{
    //    return padPos.GetAxis(padPosHandType);
    //}

    //bool GetPressPadDown()
    //{
    //    return pressPad.GetStateDown(pressPadHandType);
    //}

    private void Start()
    {
        if (player == null && GameCtrl.PlayerUnit != null)
        {
            player = GameCtrl.PlayerUnit;
            skillTable = player.SkillTable;
        }
        GameCtrl.PlayerUnitChangeEvent.AddListener(UpdatePlayer);
        EventMgr.KeyDownEvent.AddListener(SwitchCellListener);
        EventMgr.MouseButtonDownEvent.AddListener(CellMouseBTNDown);
        EventMgr.MouseButtonUpEvent.AddListener(CellMouseBTNUp);
    }

    private void Update()
    {
        if (player == null)
            return;

        //if (GameCtrl.IsVR)
        //{
        //    if (GetTriggerDown())
        //    {
        //        EventMgr.MouseButtonDownEvent.OnTrigger(new EventMgr.MouseButtonDownEventInfo(0));
        //    }
        //    if (GetTriggerUp())
        //    {
        //        EventMgr.MouseButtonUpEvent.OnTrigger(new EventMgr.MouseButtonUpEventInfo(0));
        //    }
        //    if (GetPressPadDown())
        //    {
        //        Vector2 pos = GetPadPos();
        //        int nextIndex = -1;
        //        KeyCode key;
        //        if (pos.y > 0.7f)
        //        {
        //            nextIndex = index + 1;
        //            if (nextIndex > 3)
        //                nextIndex -= 3;
        //        }
        //        else if (pos.y < -0.7f)
        //        {
        //            nextIndex = index - 1;
        //            if (nextIndex < 1)
        //                nextIndex += 3;
        //        }
        //        if (nextIndex != -1)
        //        {
        //            switch (nextIndex)
        //            {
        //                case 1:
        //                    key = KeyCode.Alpha1;
        //                    break;
        //                case 2:
        //                    key = KeyCode.Alpha2;
        //                    break;
        //                case 3:
        //                    key = KeyCode.Alpha3;
        //                    break;
        //                default:
        //                    key = KeyCode.Alpha1;
        //                    break;
        //            }
        //            SwitchCellListener(new EventMgr.KeyDownEventInfo(key));
        //            index = nextIndex;
        //        }
        //    }
        //}

    }



    private void SwitchCellListener(EventMgr.KeyDownEventInfo info)
    {
        if (player == null)
            return;
        if (GameCtrl.CursorOnGUI)
            return;

        int index = -1;
        switch (info.keyCode)
        {
            case KeyCode.Alpha1:
                index = 1;
                break;
            case KeyCode.Alpha2:
                index = 2;
                break;
            case KeyCode.Alpha3:
                index = 3;
                break;
        }
        if (GameCtrl.IsOnlineGame)
        {
            if (index != -1)
                DataSync.SyncSwitchSkill(player, Gamef.SystemTimeInMillisecond, index);
        }
        else
        {
            if (index != -1)
                skillTable.SwitchCell(index);
        }
    }

    private void CellMouseBTNDown(EventMgr.MouseButtonDownEventInfo info)
    {
        if (player == null)
            return;
        if (GameCtrl.CursorOnGUI)
            return;
        if (GameCtrl.IsOnlineGame)
        {
            lock (castingMutex)
            {
                if (!expectedCasting)
                {
                    expectedCasting = true;
                    DataSync.SyncMouseButton0Down(player, Gamef.SystemTimeInMillisecond);
                }
            }
        }
        else
        {
            // 为技能设置施法时刻
            skillTable.CurrentCell.SetInstant(Gamef.SystemTimeInMillisecond);
            // 为技能设置目标
            if (skillTable.CurrentSkill.Data.IsTracking && skillTable.CurrentSkill is ITracking it)
            {
                it.Target = AimController.Instance.Target;
            }
            skillTable.CurrentCell.Start();
        }
    }

    private void CellMouseBTNUp(EventMgr.MouseButtonUpEventInfo info)
    {
        if (player == null)
            return;
        if (GameCtrl.CursorOnGUI)
            return;
        if (GameCtrl.IsOnlineGame)
        {
            lock (castingMutex)
            {
                if (expectedCasting)
                {
                    expectedCasting = false;
                    DataSync.SyncMouseButton0Up(player, Gamef.SystemTimeInMillisecond);
                }
            }
        }
        else
        {
            skillTable.CurrentCell.SetInstant(Gamef.SystemTimeInMillisecond);
            skillTable.CurrentCell.Stop();
        }
    }
}


public partial class GameCtrl
{
    private void CheckInputForSkillTable()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventMgr.KeyDownEvent.OnTrigger(new EventMgr.KeyDownEventInfo(KeyCode.Alpha1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventMgr.KeyDownEvent.OnTrigger(new EventMgr.KeyDownEventInfo(KeyCode.Alpha2));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventMgr.KeyDownEvent.OnTrigger(new EventMgr.KeyDownEventInfo(KeyCode.Alpha3));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            EventMgr.MouseButtonDownEvent.OnTrigger(new EventMgr.MouseButtonDownEventInfo(0));
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            EventMgr.MouseButtonUpEvent.OnTrigger(new EventMgr.MouseButtonUpEventInfo(0));
        }
    }
}