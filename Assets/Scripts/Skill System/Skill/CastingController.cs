using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Unit))]
/// <summary>
/// 施法控制器。仅挂载在玩家单位上。用于将玩家的硬件输入转化为对SkillTable的指令。
/// </summary>
public class CastingController : MonoBehaviour
{
    public Unit player;
    ISkillTable skillTable;

    private void Awake()
    {
        if (player == null)
        {
            player = GetComponent<Unit>();
        }
    }

    private void Start()
    {
        skillTable = player.SkillTable;
        
        EventMgr.KeyDownEvent.AddListener(SwitchCellListener);
        EventMgr.MouseButtonDownEvent.AddListener(CellMouseBTNDown);
        EventMgr.MouseButtonUpEvent.AddListener(CellMouseBTNUp);
    }

    private void SwitchCellListener(EventMgr.KeyDownEventInfo info)
    {
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
        skillTable.CurrentCell.OnMouseButtonDown();
    }

    private void CellMouseBTNUp(EventMgr.MouseButtonUpEventInfo info)
    {
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