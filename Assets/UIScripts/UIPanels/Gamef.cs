using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Gamef
{
    public static void PercentageDisplay(float percent, PlayerInfoGamePanel panel)
    {
        int integerPart = (int)percent * panel.imagePart.Length;
        float fractionalPart = percent - integerPart;
        int i = 0, j = 0;
        for (i = 0; i < integerPart; i++)
        {
            panel.imagePart[i].fillAmount = 1;
        }
        panel.imagePart[i + 1].fillAmount = fractionalPart;
        for (j = i + 2; j < panel.imagePart.Length - 1; j++)
        {
            panel.imagePart[j].fillAmount = 0;
        }
    }
}
