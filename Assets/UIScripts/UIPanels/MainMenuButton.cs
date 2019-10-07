using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuButton : MonoBehaviour
{
    public void OnButtonClickDown(Transform transform)
    {
        transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.5f);
    }

    public void OnButtonClickUp(Transform transform)
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
}
