using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingCanvasLeft : MonoBehaviour
{
    public static FloatingCanvasLeft Instance { get; private set; }
    public float tRate = 1f;
    public float cRate = 1f;
    public float speed = 1f;
    public Image[] fillImages;
    float[] fillRates;
    [Header("Skill Image Settings")]
    public AnimationClip fadeInClip;
    public AnimationClip fadeOutClip;
    public CanvasGroup[] canvasGroups = new CanvasGroup[3];
    public Animation[] SkillAnims = new Animation[3];
    public Image[] SkillImageBgs = new Image[3];
    public Image[] SkillImages = new Image[3];
    public int currentIndex = 1;

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

    public void SetSkillImage(int skillIndex, Sprite image)
    {
        if (image.Equals(SkillImages[skillIndex - 1].sprite))
            return;
        SkillImages[skillIndex - 1].sprite = image;
        SkillImageBgs[skillIndex - 1].sprite = image;
    }
    private readonly object switchMutex = new object();
    public void Switch2Skill(int skillIndex)
    {
        lock (switchMutex)
        {
            if (skillIndex == currentIndex)
                return;
            // fade in
            canvasGroups[skillIndex - 1].alpha = 1f;
            SkillAnims[skillIndex - 1].clip = fadeInClip;
            SkillAnims[skillIndex - 1].Play();
            // fade out
            canvasGroups[currentIndex - 1].alpha = 0f;
            SkillAnims[currentIndex - 1].clip = fadeOutClip;
            SkillAnims[currentIndex - 1].Play();
            // update
            currentIndex = skillIndex;
        }
    }

    public void SetCooldown(float cooldown)
    {
        lock (switchMutex)
        {
            StartCoroutine(Cooling(currentIndex, cooldown));
        }
    }

    IEnumerator Cooling(int index, float cooldown)
    {
        float rate = 1f / cooldown;
        float timer = 0f;
        Image image = SkillImages[index - 1];
        image.fillAmount = 0f;
        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            image.fillAmount = timer * rate;
            yield return new WaitForEndOfFrame();
        }
        image.fillAmount = 1f;
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
