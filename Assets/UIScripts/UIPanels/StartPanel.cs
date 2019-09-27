using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    public AudioSource audioSource;
    public float endVolume = 1.0f;
    private float durationVolume = 10.0f;
    public Text text;
        
    public override void OnEnter()
    {
        base.OnEnter();
        audioSource.Play();
        audioSource.DOFade(endVolume, durationVolume);
    }
}
