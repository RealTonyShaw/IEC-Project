using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    public static DeathPanel Instance { get; private set; }
    public Animation GaussianBlurAnim;
    public Animation DeathPanelAlphaAnim;

    private void Awake()
    {
        Instance = this;
    }

    public void BeginDeath()
    {
        if (GaussianBlurAnim == null)
        {
            GaussianBlurAnim = Camera.main.gameObject.GetComponent<Animation>();
        }
        GaussianBlurAnim.Play();
        DeathPanelAlphaAnim.Play();
        Crosshair.SetState(false);
        Gamef.DelayedExecution(delegate
        {
            Crosshair.SetState(true);
        }, DeathPanelAlphaAnim.clip.length);
    }
}
