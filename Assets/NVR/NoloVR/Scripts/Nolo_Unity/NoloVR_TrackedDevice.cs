/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_TrackedDevice.cs
 *   
*************************************************************/

using UnityEngine;

public class NoloVR_TrackedDevice : MonoBehaviour
{

    public NoloDeviceType deviceType;
    private GameObject vrCamera;
    void Start()
    {
        vrCamera = NoloVR_System.GetInstance().VRCamera;
    }
    void Update()
    {
        if (NoloVR_Playform.GetInstance().GetPlayformError() != NoloError.None)
        {
            return;
        }
        UpdatePose();
    }


    void UpdatePose()
    {
        var pose = NoloVR_Controller.GetDevice(deviceType).GetPose();

        if (deviceType != NoloDeviceType.Hmd)
        {
            transform.localPosition = pose.pos;
            transform.localRotation = pose.rot;
        }
        else
        {
            if (vrCamera == null)
            {
                Debug.LogError("Not find your vr camera");
                return;
            }
            transform.localRotation = pose.rot;
            var cameraLoaclPosition = transform.localRotation * vrCamera.transform.localPosition;
            transform.localPosition = pose.pos - cameraLoaclPosition;
        }
    }
}
