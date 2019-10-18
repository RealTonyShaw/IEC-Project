using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    public static DeathPanel Instance { get; private set; }
    public Animation GaussianBlurAnim;
    public Animation DeathPanelAlphaAnim;
    public Animation CrossHairFadeAnim;

    private void Awake()
    {
        Instance = this;
    }

    public void BeginDeath()
    {
        GaussianBlurAnim.Play();
        DeathPanelAlphaAnim.Play();
        CrossHairFadeAnim.Play();
    }
}
