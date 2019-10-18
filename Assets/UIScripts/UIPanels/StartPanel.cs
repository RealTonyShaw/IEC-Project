using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    public AudioSource audioSource;
    public Text text;
        
    public override void OnEnter()
    {
        base.OnEnter();
        audioSource.Play();
    }
}
