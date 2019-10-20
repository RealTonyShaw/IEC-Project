using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : BasePanel
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnSingleGameClick(AudioSource audioSource)
    {
        UIManager.Instance.PushPanel(PanelType.SingleGame);
        audioSource.Play();
    }

    public void OnMultiGameClick(AudioSource audioSource)
    {
        UIManager.Instance.PushPanel(PanelType.MultiGame);
        audioSource.Play();
    }

    public void OnSetUpGameClick(AudioSource audioSource)
    {
        UIManager.Instance.PushPanel(PanelType.SetUpGame);
        audioSource.Play();
    }

    public void OnAboutUsClick(AudioSource audioSource)
    {
        UIManager.Instance.PushPanel(PanelType.AboutUs);
        audioSource.Play();
    }

    public void OnExitGameClick(AudioSource audioSource)
    {
        audioSource.Play();
        Gamef.DelayedExecution(Application.Quit, 0.5f);
    }
}
