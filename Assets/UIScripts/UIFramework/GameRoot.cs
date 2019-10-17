using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.UIStackClean();
        UIManager.Instance.PushPanel(PanelType.Start);
    }

    bool isEnter = false;
    private void Update()
    {
        if (!isEnter)
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                isEnter = true;
                UIManager.Instance.PopPanel(PanelType.Start);
                UIManager.Instance.PushPanel(PanelType.MainMenu);
            }
    }
}
