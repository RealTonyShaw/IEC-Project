using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public partial class MultiGamePanel : BasePanel
{
    public InputField host;
    public InputField userName;
    public InputField password;
    public Transform trans;

    public override void OnEnter()
    {
        canvasGroup.DOFade(1, 2f);
        trans.DOScale(new Vector3(1, 1, 1), 2f);
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
        trans.DOScale(new Vector3(0, 0, 0), 2f);
    }

    public override void OnPause()
    {
        base.OnPause();
        trans.DOScale(new Vector3(0, 0, 0), 2f);
    }

    public override void OnResume()
    {
        base.OnResume();
        trans.DOScale(new Vector3(1, 1, 1), 2f);
    }

    public void OnExitClick(AudioSource audioSource)
    {
        UIManager.Instance.PopPanel(PanelType.MultiGame);
        audioSource.Play();
    }

    public void OnButtonClickDown(Transform transform)
    {
        transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.5f);
    }

    public void OnButtonClickUp(Transform transform)
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }

    public void OnHostChanged()
    {
        ClientLauncher.Instance.host = host.text;
    }

    public void OnRegisterClick(AudioSource audioSource)
    {
        UIManager.Instance.PushPanel(PanelType.Register);
        audioSource.Play();
    }
}
