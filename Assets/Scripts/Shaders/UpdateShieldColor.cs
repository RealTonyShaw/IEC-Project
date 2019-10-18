using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateShieldColor : MonoBehaviour
{
    [Range(-1000, 1000)]
    public float m_ColorUpdateSpeed;

    private Color mainColor = Color.blue;
    private Material magicMaterial;

    private float tempColor, startColorValue = 0.6025f, endColorValue = 1.7982f;
    private float[] color;
    private int colorIndex = 1;

    void Start()
    {
        color = new float[3];
        magicMaterial = GetComponent<Renderer>().material;
        //mainColor = magicMaterial.GetColor("_Color");
        color[0] = mainColor.r;
        color[1] = mainColor.g;
        color[2] = mainColor.b;
    }

    void Update()
    {
        tempColor += m_ColorUpdateSpeed * Time.deltaTime / 1000;
        if (tempColor > endColorValue)
        {
            tempColor = endColorValue;
            m_ColorUpdateSpeed *= -1;
        }
        if (tempColor < startColorValue)
        {
            tempColor = startColorValue;
            m_ColorUpdateSpeed *= -1;
            colorIndex++;
            colorIndex = colorIndex % 3;
        }
        color[colorIndex] = tempColor;
        mainColor.r = color[0];
        mainColor.g = color[1];
        mainColor.b = color[2];
        magicMaterial.SetColor("_Color", mainColor);
    }
}
