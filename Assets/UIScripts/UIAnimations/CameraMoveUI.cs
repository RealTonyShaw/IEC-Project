using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveUI : MonoBehaviour
{
    [SerializeField]
    private float distance = 20.0f;
    [SerializeField]
    private float horizontalSpeed = 10;
    [SerializeField]
    private float verticalSpeed = 5;
    [SerializeField]
    private Transform centerTransform;

    private void Update()
    {
        transform.position = centerTransform.position + new Vector3(Mathf.Sin(Time.time * Mathf.Deg2Rad * horizontalSpeed), Mathf.Sin(Time.time * Mathf.Deg2Rad * verticalSpeed), Mathf.Cos(Time.time * Mathf.Deg2Rad * horizontalSpeed)) * Mathf.Abs(Mathf.Sin(Time.time * 0.125f)) * distance;
        transform.LookAt(centerTransform);
    }
}
