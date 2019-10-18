using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoPanel : MonoBehaviour
{
    public float delay;
    public string scene;
    public AsyncOperation ao;
    public CanvasGroup canvasGroup;
    public Animation anim;
    public AnimationClip FadeInClip;
    public AnimationClip FadeOutClip;

    bool startLoading = false;
    private void Start()
    {
        canvasGroup.alpha = 0f;
        anim.clip = FadeInClip;
        anim.Play();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(DelayLoad());
    }

    IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(delay);
        ao = SceneManager.LoadSceneAsync(scene);
        startLoading = true;
    }

    private void Update()
    {
        if (startLoading && ao.isDone)
        {
            anim.clip = FadeOutClip;
            anim.Play();
            StartCoroutine(DelayDisable(FadeOutClip.length + 0.2f));
        }
    }

    IEnumerator DelayDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
