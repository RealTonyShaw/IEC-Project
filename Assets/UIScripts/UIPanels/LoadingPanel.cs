using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public CanvasGroup canvas;
    [Header("Animation Settings")]
    public Animation Anim;
    public AnimationClip FadeOutClip;
    public AnimationClip FadeInClip;

    public void StartLoading()
    {
        canvas.blocksRaycasts = true;
        Anim.clip = FadeInClip;
        Anim.Play();
    }
    public void StopLoading()
    {
        canvas.blocksRaycasts = false;
        Anim.clip = FadeOutClip;
        Anim.Play();
    }
}
