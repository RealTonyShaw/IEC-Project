using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    Ray ray;
    public GameObject aimingCrosshair;

    void Start()
    {
        Color tempColor = aimingCrosshair.GetComponent<Image>().color;
        tempColor.a = 0f;
        aimingCrosshair.GetComponent<Image>().color = tempColor;
    }

    void Update()
    {
        ray = CameraGroupController.Instance.MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Detecting");
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Detected");
                Emerging();
            }
            else
                Disappearing();
        }
        Disappearing();
    }

    void Emerging()
    {
        Color tempColor = aimingCrosshair.GetComponent<Image>().color;
        tempColor.a = 1f;
        aimingCrosshair.GetComponent<Image>().color = tempColor;
        Debug.Log("Emerging");
    }

    void Disappearing()
    {
        Color tempColor = aimingCrosshair.GetComponent<Image>().color;
        tempColor.a -= 0.1f;
        aimingCrosshair.GetComponent<Image>().color = tempColor;
        Debug.Log("Disappearing");
    }

    float GetAlphaOfCrosshair()
    {
        return aimingCrosshair.GetComponent<Image>().color.a;
    }
}
