using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodStripControl : MonoBehaviour
{
    private float barUpLength = 3f;
    public Slider BloodStrip;

    public void Update()
    {
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + barUpLength, transform.position.z);
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector3 screenPos = worldPos;
        BloodStrip.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }
}
