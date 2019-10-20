using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscPanel : BasePanel
{
    public static EscPanel Instance { get; private set; }
    public static bool IsEnter { get; private set; } = false;

    bool back = false;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        IsEnter = true;
        GameCtrl.CursorOnGUI = true;
        Crosshair.SetState(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        IsEnter = false;
        GameCtrl.CursorOnGUI = false;
        if (!back)
            Crosshair.SetState(true);
    }

    public void OnResumeClick()
    {
        UIManager.Instance.PopPanel(PanelType.Esc);
    }

    public void OnBack2MenuClick()
    {
        back = true;
        UIManager.Instance.PopPanel(PanelType.Esc);
        GameCtrl.Instance.Back2Menu();
        GameCtrl.CursorOnGUI = false;
    }
}
