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
    public string endString = "Of course, listening to music is also good.";
    public string startString = "Press Enter to continue!";
    private float durationString = 5.0f;
    private int state = 0;
        
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
}
