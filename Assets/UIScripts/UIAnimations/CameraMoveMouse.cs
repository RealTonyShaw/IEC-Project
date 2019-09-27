using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMouse : MonoBehaviour
{
    public Camera camra;

    public void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        Debug.Log("x: " + x);
    }
}
