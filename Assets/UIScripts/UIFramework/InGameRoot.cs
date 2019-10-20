using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRoot : MonoBehaviour
{
    public static InGameRoot Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.UIStackClean();
        UIManager.Instance.RestartDictionary(PanelType.Esc);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (EscPanel.IsEnter)
            {
                UIManager.Instance.PopPanel(PanelType.Esc);
            }
            else
            {
                UIManager.Instance.PushPanel(PanelType.Esc);
            }
        }
    }
}
