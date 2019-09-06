using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MultiGamePanel : BasePanel
{
    public Transform trans;

    public override void OnEnter()
    {
        canvasGroup.DOFade(1, 2f);
        trans.DORotate(new Vector3(0, 0, 0), 2f, RotateMode.FastBeyond360);
        StartCoroutine(DelayEnable());
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1.8f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public override void OnExit()
    {
        canvasGroup.DOFade(0, 2);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        trans.DORotate(new Vector3(0, 180, 0), 2, RotateMode.FastBeyond360);
    }

    public void OnExitClick()
    {
        UIManager.Instance.PopPanel(PanelType.MultiGame);
    }
}
