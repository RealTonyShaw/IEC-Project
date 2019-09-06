using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuPanel : BasePanel
{
    public AudioSource audioSource;
    public float endVolume = 1.0f;
    private float durationVolume = 10.0f;
    public Transform panelTransform;
    public CanvasGroup panelCanvasGroup;
    public Transform mainMenuTransform;

    public override void OnEnter()
    {
        base.OnEnter();
        audioSource.Play();
        audioSource.DOFade(endVolume, durationVolume);
    }

    public override void OnExit()
    {
        base.OnExit();
        audioSource.DOFade(0, durationVolume);
    }

    public override void OnPause()
    {
        base.OnPause();
        panelTransform.DOMoveX(0, 2);
        panelCanvasGroup.DOFade(0, 1);
        mainMenuTransform.DOMoveX(1800, 2);
    }

    public override void OnResume()
    {
        mainMenuTransform.DOMoveX(740, 1);
        panelCanvasGroup.DOFade(1, 1);
        panelTransform.DOMoveX(915, 1);
        StartCoroutine(DelayEnable());
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnSingleGameClick()
    {
        UIManager.Instance.PushPanel(PanelType.SingleGame);
    }

    public void OnMultiGameClick()
    {
        UIManager.Instance.PushPanel(PanelType.MultiGame);
    }

    public void OnSetUpGameClick()
    {
        UIManager.Instance.PushPanel(PanelType.SetUpGame);
    }

    public void OnAboutUsClick()
    {
        UIManager.Instance.PushPanel(PanelType.AboutUs);
    }

    public void OnPlayerInfoClick()
    {
        UIManager.Instance.PushPanel(PanelType.PlayerInfo);
    }

    public void OnComplaintToUsClick()
    {
        UIManager.Instance.PushPanel(PanelType.ComplaintToUs);
    }

    public void OnExitGameClick()
    {

    }
}
