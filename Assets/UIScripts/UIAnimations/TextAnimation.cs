using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private float scintillationInterval = 2;
    private float alpha = 1;
    private Color textColor = Color.white;
    private bool forward = false;

    private void Update()
    {
        if (forward)
        {
            alpha += Time.smoothDeltaTime * scintillationInterval;
            if (alpha >= 1)
            {
                forward = !forward;
            }
        }
        else
        {
            alpha -= Time.smoothDeltaTime * scintillationInterval;
            if (alpha <= 0)
            {
                forward = !forward;
            }
        }
        textColor.a = alpha;
        text.color = textColor;
    }
}
