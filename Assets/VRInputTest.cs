using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRInputTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean triggerAction; // 2
    public SteamVR_Action_Vector2 touchPadPosAction; // 3
    public SteamVR_Action_Pose pose;

    public bool GetTrigger()
    {
        return triggerAction.GetState(handType);
    }

    public bool GetTriggerUp()
    {
        return triggerAction.GetStateUp(handType);
    }

    public Vector2 GetTouchPadPos()
    {
        return touchPadPosAction.GetAxis(handType);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetTrigger())
        {
            Debug.Log("Triggering~");
        }
        if (GetTriggerUp())
        {
            Debug.Log("Trigger UPPPP!!!");
        }
        Debug.Log("Pos = " + GetTouchPadPos());
        //if (handType == SteamVR_Input_Sources.LeftHand)
        //{

        //    Debug.Log("Hmd rot = " + pose.GetLocalRotation(SteamVR_Input_Sources.LeftHand));
        //}
    }
}
