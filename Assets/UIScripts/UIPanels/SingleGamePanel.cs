﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SingleGamePanel : BasePanel
{
    public Transform trans;
    public AudioSource audioSource;

    public override void OnEnter()
    {
        canvasGroup.DOFade(1, 2f);
        trans.DORotate(new Vector3(0, 0, 0), 2f, RotateMode.FastBeyond360);
        StartCoroutine(DelayEnable());
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSecondsRealtime(1f);
        audioSource.Play();
        yield return new WaitForSeconds(0.8f);
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
        UIManager.Instance.PopPanel(PanelType.SingleGame);
    }

    public void OnButtonClickDown(Transform transform)
    {
        transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.5f);
    }

    public void OnButtonClickUp(Transform transform)
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }

    public void OnEnterButtonClick()
    {
        GameCtrl.Instance.StartSingleGame();
    }
}
