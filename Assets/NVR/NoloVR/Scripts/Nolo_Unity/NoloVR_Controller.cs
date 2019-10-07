/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Controller.cs
 *   
*************************************************************/

using UnityEngine;
using NoloClientCSharp;

public class NoloVR_Controller {

    public static bool isTurnAround = false;
    static Vector3 recPosition = Vector3.zero;

    //button mask
    public class ButtonMask
    {
        public const uint TouchPad = 1 << (int)NoloButtonID.TouchPad;
        public const uint Trigger = 1 << (int)NoloButtonID.Trigger;
        public const uint Menu = 1 << (int)NoloButtonID.Menu;
        public const uint System = 1 << (int)NoloButtonID.System;
        public const uint Grip = 1 << (int)NoloButtonID.Grip;

        public const uint TouchPadUp = 1 << (int)NoloButtonID.TouchPadUp;
        public const uint TouchPadDown = 1 << (int)NoloButtonID.TouchPadDown;
        public const uint TouchPadLeft = 1 << (int)NoloButtonID.TouchPadLeft;
        public const uint TouchPadRight = 1 << (int)NoloButtonID.TouchPadRight;
    }
    //touch mask
    public class TouchMask
    {
        public const uint TouchPad = 1 << (int)NoloTouchID.TouchPad;
    }
    //device message
    public class NoloDevice
    {
        public NoloDevice(int num)
        {
            index = num;
        }
        public int index { get; private set; }
        public Nolo_Transform GetPose()
        {
            Update();
            return pose;
        }

        public bool GetNoloButtonPressed(uint buttonMask)
        {
            Update();
            return (controllerStates.buttons & buttonMask) != 0;
        }
        public bool GetNoloButtonDown(uint buttonMask)
        {
            Update();
            return (controllerStates.buttons & buttonMask) != 0 && (preControllerStates.buttons & buttonMask) == 0;
        }
        public bool GetNoloButtonUp(uint buttonMask)
        {
            Update();
            return (controllerStates.buttons & buttonMask) == 0 && (preControllerStates.buttons & buttonMask) != 0;
        }

        public bool GetNoloButtonPressed(NoloButtonID button)
        {
            return GetNoloButtonPressed((uint)1 << (int)button);
        }
        public bool GetNoloButtonDown(NoloButtonID button)
        {
            return GetNoloButtonDown((uint)1 << (int)button);
        }
        public bool GetNoloButtonUp(NoloButtonID button)
        {
            return GetNoloButtonUp((uint)1 << (int)button);
        }

        public bool GetNoloTouchPressed(uint touchMask)
        {
            Update();
            return (controllerStates.touches & touchMask) !=0;
        }
        public bool GetNoloTouchDown(uint touchMask)
        {
            Update();
            return (controllerStates.touches & touchMask) != 0 && (preControllerStates.touches & touchMask) == 0;
        }
        public bool GetNoloTouchUp(uint touchMask)
        {
            Update();
            return (controllerStates.touches & touchMask) == 0 && (preControllerStates.touches & touchMask) != 0;
        }

        public bool GetNoloTouchPressed(NoloTouchID touch)
        {
            return GetNoloTouchPressed((uint)1 << (int)touch);
        }
        public bool GetNoloTouchDown(NoloTouchID touch)
        {
            return GetNoloTouchDown((uint)1 << (int)touch);
        }
        public bool GetNoloTouchUp(NoloTouchID touch)
        {
            return GetNoloTouchUp((uint)1 << (int)touch);
        }

        //touch axis return vector2 x(-1~1)y(-1,1)
        public Vector2 GetAxis(NoloTouchID axisIndex = NoloTouchID.TouchPad)
        {
            Update();
            if (axisIndex == NoloTouchID.TouchPad)
            {
                return new Vector2(controllerStates.touchpadAxis.x, controllerStates.touchpadAxis.y);
            }
            if (axisIndex == NoloTouchID.Trigger)
            {
                return new Vector2(controllerStates.rAxis1.x, controllerStates.rAxis1.y);
            }
            return Vector2.zero;
        }

        private Nolo_ControllerStates controllerStates, preControllerStates;
        private Nolo_Transform pose;
        private int preFrame = -1;
        public void Update()
        {
            if (Time.frameCount != preFrame)
            {
                preFrame = Time.frameCount;
                preControllerStates = controllerStates;
                if (NoloVR_Playform.GetInstance().GetPlayformError() == NoloError.None && NoloVR_Playform.GetInstance().GetAuthentication())
                {
                    controllerStates = NoloVR_Plugins.GetControllerStates(index);
                    float yaw = real_yaw * 57.3f;
                    pose = NoloVR_Plugins.GetPose(index);
                    if (index == 0)
                    {
                        //pose.pos += pose.rot * new Vector3(0, 0.08f, 0.062f);
                        pose.rot = Quaternion.Euler(new Vector3(0, -yaw, 0));
                        //pose.pos -= pose.rot * new Vector3(0, 0.08f, 0.062f);
                    }
                    if (isTurnAround)
                    {
                        if (NoloVR_Controller.recPosition == Vector3.zero)
                        {
                            NoloVR_Controller.recPosition = NoloVR_Plugins.GetPose(0).pos;
                        }
                        var rot = pose.rot.eulerAngles;
                        rot += new Vector3(0, 180 + yaw, 0);
                        pose.rot = Quaternion.Euler(rot);
                        Vector3 revec = Quaternion.Euler(new Vector3(0, 180 + yaw, 0)) * pose.pos + NoloVR_Controller.recPosition;
                        pose.pos.x = revec.x;
                        pose.pos.z = revec.z;
                        pose.vecVelocity.x = -pose.vecVelocity.x;
                        pose.vecVelocity.z = -pose.vecVelocity.z;
                        return;
                    }
                }
            }
        }

        //HapticPulse  parameter must be in 0~100
        public void TriggerHapticPulse(int intensity)
        {
            if (NoloVR_Playform.GetInstance().GetPlayformError() == NoloError.None)
            {
                NoloVR_Plugins.TriggerHapticPulse(index, intensity);
            }
        }
    }
    
    //device manager
    public static NoloDevice[] devices;
    public static NoloDevice GetDevice(NoloDeviceType deviceIndex)
    {
        if (devices == null)
        {
            devices = new NoloDevice[NoloVR_Plugins.trackedDeviceNumber];
            for (int i = 0; i < devices.Length; i++)
            {
                devices[i] = new NoloDevice(i);
            }
        }
        return devices[(int)deviceIndex];
    }
    public static NoloDevice GetDevice(NoloVR_TrackedDevice trackedObject)
    {
        return GetDevice(trackedObject.deviceType);
    }

    //turn around events
    static void TurnAroundEvents(params object[] args)
    {
        isTurnAround = !isTurnAround;
    }

    static float real_yaw = 0;
    static float PI = 3.1415926f;
    //RecenterLeft events
    static void RecenterLeftEvents(params object[] args)
    {
        Vector3 handPosLeft = NoloVR_Plugins.GetPose(1).pos;
        Vector3 handPosRight = NoloVR_Plugins.GetPose(2).pos;
        Vector3 HeadPos = NoloVR_Plugins.GetPose(0).pos;
        Vector3 HandPos = NoloVR_Plugins.GetPose(1).pos;
        if (Vector3.Distance(handPosLeft, handPosRight)<0.2f)
        {
            HandPos = (handPosLeft + handPosRight) / 2;
        }
        if ((HandPos.x - HeadPos.x) > 0)
        {
            real_yaw = Mathf.Atan((HandPos.z - HeadPos.z) / (HandPos.x - HeadPos.x)) - PI / 2;//真实航向角
        }
        else if ((HandPos.x - HeadPos.x) < 0)
        {
            real_yaw = PI / 2 + Mathf.Atan((HandPos.z - HeadPos.z) / (HandPos.x - HeadPos.x));//真实航向角
        }
    }
    //RecenterRight events
    static void RecenterRightEvents(params object[] args)
    {
        Vector3 handPosLeft = NoloVR_Plugins.GetPose(1).pos;
        Vector3 handPosRight = NoloVR_Plugins.GetPose(2).pos;
        Vector3 HandPos = NoloVR_Plugins.GetPose(2).pos;
        Vector3 HeadPos = NoloVR_Plugins.GetPose(0).pos;
        if (Vector3.Distance(handPosLeft, handPosRight) < 0.2f)
        {
            HandPos = (handPosLeft + handPosRight) / 2;
        }
        if ((HandPos.x - HeadPos.x) > 0)
        {
            real_yaw = Mathf.Atan((HandPos.z - HeadPos.z) / (HandPos.x - HeadPos.x)) - PI / 2;//真实航向角
        }
        else if ((HandPos.x - HeadPos.x) < 0)
        {
            real_yaw = PI / 2 + Mathf.Atan((HandPos.z - HeadPos.z) / (HandPos.x - HeadPos.x));//真实航向角
        }
    }
    public static void Listen()
    {
        NOLO_Events.Listen(NOLO_Events.EventsType.TurnAround, TurnAroundEvents);
        NOLO_Events.Listen(NOLO_Events.EventsType.RecenterLeft, RecenterLeftEvents);
        NOLO_Events.Listen(NOLO_Events.EventsType.RecenterRight, RecenterRightEvents);
    }
    public static void Remove()
    {
        NOLO_Events.Remove(NOLO_Events.EventsType.TurnAround, TurnAroundEvents);
        NOLO_Events.Remove(NOLO_Events.EventsType.RecenterLeft, RecenterLeftEvents);
        NOLO_Events.Remove(NOLO_Events.EventsType.RecenterRight, RecenterRightEvents);
    }

}
