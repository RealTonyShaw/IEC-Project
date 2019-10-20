using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.UIStackClean();
        UIManager.Instance.RestartDictionaryExcept(PanelType.Esc);
        StartCoroutine(DelayPush(PanelType.Start, 2f));
        Gamef.DelayedExecution(delegate()
        {
            isInit = true;
        }, 2.6f);
    }

    IEnumerator DelayPush(PanelType type, float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.PushPanel(type);
    }

    bool isInit = false;
    bool isEnter = false;
    private void Update()
    {
        if (isInit && !isEnter)
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                isEnter = true;
                UIManager.Instance.PopPanel(PanelType.Start);
                UIManager.Instance.PushPanel(PanelType.MainMenu);
            }
    }
}
