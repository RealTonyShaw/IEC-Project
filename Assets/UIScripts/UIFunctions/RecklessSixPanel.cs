using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecklessSixPanel : MonoBehaviour
{
    public Animation Anim;
    public float fadeOutDelay = 6f;

    private void Start()
    {
        Gamef.DelayedExecution(delegate
        {
            Anim.Play();
            Destroy(gameObject, 2f);
        }, fadeOutDelay);
    }
}
