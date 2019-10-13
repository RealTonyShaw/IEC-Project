using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasFollowing : MonoBehaviour
{
    // 以哪一个物体为参考系。Canvas会在World Space追踪该物体并达到相对于该物体的固定位置处
    [Header("Settings")]
    public Transform Origin = null;
    public Vector3 localEulerAngles = new Vector3(0f, 225f, 0f);
    public Vector3 localPosition = new Vector3(1f, 0f, 1f);
    private Quaternion localRotation;
    [Header("Parameters")]
    public float angularConst = 8f;
    public float normalConst = 25f;
    public float tangentialConst = 5f;

    private void Start()
    {
        localRotation = Quaternion.Euler(localEulerAngles);
        if (Origin == null)
        {
            Origin = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (Origin == null)
        {
            Debug.LogWarning("Canvas Need to have an origin");
            return;
        }
        //Origin.TransformDirection
    }
}
