using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public partial class RegisterPanel : BasePanel
{
    public InputField host;
    public InputField userName;
    public InputField password;
    public InputField repeatPw;
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

    public void OnExitClick(AudioSource audioSource)
    {
        UIManager.Instance.PopPanel(PanelType.Register);
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

    public void OnHostEdited()
    {
        ClientLauncher.Instance.host = host.text;
    }
}
