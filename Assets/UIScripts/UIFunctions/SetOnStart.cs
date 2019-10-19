using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOnStart : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public Vector3 scale;
    public float alpha;
    public Vector3 rotation;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = scale;
        canvasGroup.alpha = alpha;
        transform.eulerAngles = rotation;
    }
}
