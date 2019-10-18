using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEnableAndDisable : MonoBehaviour
{
    public GameObject[] OnDeathEnable;
    public GameObject[] OnDeathDisable;
    public FadeSetting[] OnDeathFade;
    public GameObject[] OnDisableResetActive;
    private bool[] activeStates;

    bool isFirst = true;
    private void OnEnable()
    {
        if (isFirst)
            isFirst = false;
        else return;

        int len = OnDisableResetActive.Length;
        activeStates = new bool[len];
        for (int i = 0; i < len; i++)
        {
            activeStates[i] = OnDisableResetActive[i].activeSelf;
        }
    }

    [System.Serializable]
    public class FadeSetting
    {
        public FadeSetting()
        {
            StopAfterFaded = true;
            DisableAfterFaded = false;
            fadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.2f, 0));
        }
        public AudioSource source;
        public AnimationCurve fadeCurve;
        public bool StopAfterFaded = true;
        public bool DisableAfterFaded = false;
    }

    private void OnDisable()
    {
        int len = OnDisableResetActive.Length;
        for (int i = 0; i < len; i++)
        {
            if (OnDisableResetActive[i].activeSelf != activeStates[i])
            {
                OnDisableResetActive[i].SetActive(activeStates[i]);
            }
        }
    }

    public void OnDeath()
    {
        foreach (var obj in OnDeathEnable)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
        foreach (var obj in OnDeathDisable)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
        foreach (var setting in OnDeathFade)
        {
            StartCoroutine(SourceFading(setting));
        }
    }

    IEnumerator SourceFading(FadeSetting setting)
    {
        if (setting.source != null)
        {
            float t = Time.time;
            float rate;
            float vol = setting.source.volume;
            do
            {
                rate = setting.fadeCurve.Evaluate(Time.time - t);
                setting.source.volume = vol * rate;
                yield return new WaitForEndOfFrame();
            } while (rate > 1e-5f);
            if (setting.StopAfterFaded)
            {
                setting.source.Stop();
                setting.source.volume = vol;
            }
            if (setting.DisableAfterFaded)
            {
                setting.source.gameObject.SetActive(false);
                setting.source.volume = vol;
            }
        }
    }
}
