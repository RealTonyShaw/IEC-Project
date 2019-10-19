using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRoot : MonoBehaviour
{
    private bool isEnter = false;

    private void Start()
    {
        UIManager.Instance.UIStackClean();
        UIManager.Instance.RestartDictionary(PanelType.Esc);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEnter)
            {
                UIManager.Instance.PopPanel(PanelType.Esc);
            }
            else
            {
                UIManager.Instance.PushPanel(PanelType.Esc);
            }
            isEnter = !isEnter;
            GameCtrl.CursorOnGUI = isEnter;
            Crosshair.SetState(!isEnter);
        }
    }
}
