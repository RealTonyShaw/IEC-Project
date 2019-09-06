using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 施法控制器。用于将玩家的硬件输入转化为对SkillTable的指令。
/// </summary>
public class CastingController : MonoBehaviour
{
    Unit player = null;
    ISkillTable skillTable;

    private void Start()
    {
        if (player == null && GameCtrl.PlayerUnit != null)
        {
            player = GameCtrl.PlayerUnit;
            skillTable = player.SkillTable;
        }

        EventMgr.KeyDownEvent.AddListener(SwitchCellListener);
        EventMgr.MouseButtonDownEvent.AddListener(CellMouseBTNDown);
        EventMgr.MouseButtonUpEvent.AddListener(CellMouseBTNUp);
    }

    private void Update()
    {
        if (player == null && GameCtrl.PlayerUnit != null)
        {
            player = GameCtrl.PlayerUnit;
            skillTable = player.SkillTable;
        }
    }

    private void SwitchCellListener(EventMgr.KeyDownEventInfo info)
    {
        if (player == null)
            return;
        switch (info.keyCode)
        {
            case KeyCode.Alpha1:
                skillTable.SwitchCell(1);
                break;
            case KeyCode.Alpha2:
                skillTable.SwitchCell(2);
                break;
            case KeyCode.Alpha3:
                skillTable.SwitchCell(3);
                break;
        }
    }

    private void CellMouseBTNDown(EventMgr.MouseButtonDownEventInfo info)
    {
        if (player == null)
            return;
        // 为技能设置施法时刻
        skillTable.CurrentCell.SetInstant(Gamef.SystemTimeInMillisecond);
        // 为技能设置目标
        if (skillTable.CurrentSkill.Data.IsTracking && skillTable.CurrentSkill is ITracking it)
        {
            it.Target = AimController.Instance.TargetForStrafeSkill;
        }
        skillTable.CurrentCell.OnMouseButtonDown();
    }

    private void CellMouseBTNUp(EventMgr.MouseButtonUpEventInfo info)
    {
        if (player == null)
            return;
        skillTable.CurrentCell.SetInstant(Gamef.SystemTimeInMillisecond);
        skillTable.CurrentCell.OnMouseButtonUp();
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