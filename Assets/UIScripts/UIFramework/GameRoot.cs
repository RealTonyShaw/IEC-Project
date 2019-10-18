using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.UIStackClean();
        StartCoroutine(DelayPush(PanelType.Start, 2f));
    }

    IEnumerator DelayPush(PanelType type, float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.PushPanel(type);
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
