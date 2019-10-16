using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Valve.VR;

public class VR_Hmd_Parent : MonoBehaviour
{
    public static Transform hmd;
    public Transform head;

    private void Awake()
    {
        hmd = head;
    }
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = -head.localPosition;
    }
    
}
