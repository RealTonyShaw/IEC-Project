using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    /// <summary>
    /// 当前面板处于进入状态，即使用状态
    /// </summary>
    public virtual void OnEnter()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 当前面板处于暂停状态，无法使用
    /// </summary>
    public virtual void OnPause()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 当前面板处于恢复状态，使用状态恢复
    /// </summary>
    public virtual void OnResume()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 当前面板处于退出状态，无法使用
    /// </summary>
    public virtual void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
