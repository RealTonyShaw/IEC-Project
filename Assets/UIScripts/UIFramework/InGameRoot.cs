using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRoot : MonoBehaviour
{
    private bool isEnter = false;

    private void Start()
    {
        UIManager.Instance.UIStackClean();
        UIManager.Instance.RestartDictionary();
    }

    private void Update()
    {
        if(!isEnter)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isEnter = true;
                UIManager.Instance.PushPanel(PanelType.Esc);
            }
        }
    }
}
