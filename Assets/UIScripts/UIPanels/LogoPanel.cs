using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoPanel : MonoBehaviour
{
    public float startDelay;
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
        Cursor.visible = false;
        canvasGroup.alpha = 0f;
        Gamef.DelayedExecution(delegate
        {
            canvasGroup.alpha = 1f;
            anim.clip = FadeInClip;
            anim.Play();
            StartCoroutine(DelayLoad());
        }, startDelay);
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
            Cursor.visible = true;
            anim.clip = FadeOutClip;
            anim.Play();
            StartCoroutine(DelayDisable(1.8f));
        }
    }

    IEnumerator DelayDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
