using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Controller : MonoBehaviour
{
    public static Transform leftHand;
    public static Transform rightHand;
    public SteamVR_Input_Sources src;

    private void Awake()
    {
        switch (src)
        {
            case SteamVR_Input_Sources.LeftHand:
                leftHand = transform;
                break;
            case SteamVR_Input_Sources.RightHand:
                rightHand = transform;
                break;
            default:
                break;
        }
    }
}
