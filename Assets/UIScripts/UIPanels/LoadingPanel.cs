using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingPanel : BasePanel
{
    public Text text;

    public override void OnEnter()
    {
        base.OnEnter();
    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForSecondsRealtime(3f);
    }

    public override void OnPause()
    {
        base.OnPause();
    }
}
