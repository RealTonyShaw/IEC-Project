using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingCanvasRight : MonoBehaviour
{
    public static FloatingCanvasRight Instance { get; private set; }
    public float tRate = 1f;
    public float cRate = 1f;
    public float speed = 1f;
    public Image[] fillImages;
    float[] fillRates;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fillRates = new float[fillImages.Length];
        for (int i = 0; i < fillRates.Length; i++)
        {
            fillRates[i] = 1f;
        }
    }

    public void SetRate(float rate)
    {
        tRate = Mathf.Clamp(rate, 0f, 1f);
    }

    private void Update()
    {
        UpdateRate();
        UpdateEachFillRate();
        UpdateEachImage();
    }

    void UpdateRate()
    {
        if (Mathf.Abs(cRate - tRate) > 1e-5f)
        {
            if (cRate < tRate)
            {
                cRate = Mathf.Clamp(cRate + speed * Time.deltaTime, cRate, tRate);
            }
            else
            {
                cRate = Mathf.Clamp(cRate - speed * Time.deltaTime, tRate, cRate);
            }
        }
    }

    void UpdateEachFillRate()
    {
        int cnt = fillRates.Length;
        float total = cRate * cnt;
        for (int i = 0; i < cnt; i++)
        {
            if (total > 1f)
            {
                fillRates[i] = 1f;
                total -= 1f;
            }
            else if (total > 0f)
            {
                fillRates[i] = total;
                total -= 1f;
            }
            else
            {
                fillRates[i] = 0f;
            }
        }
    }

    void UpdateEachImage()
    {
        float c, t;
        for (int i = 0; i < fillRates.Length; i++)
        {
            t = fillRates[i];
            c = fillImages[i].fillAmount;
            if (Mathf.Abs(t - c) > 1e-5f)
            {
                fillImages[i].fillAmount = t;
            }
        }
    }
}
