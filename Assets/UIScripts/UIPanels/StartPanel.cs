using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    public Animation anim;
    public Text text;
    public AudioSource audioSource;
        
    public override void OnEnter()
    {
        base.OnEnter();
        audioSource.DOFade(1, 9);
        anim.Play();
        GetComponent<CanvasGroup>().alpha = 1f; 
    }
}
