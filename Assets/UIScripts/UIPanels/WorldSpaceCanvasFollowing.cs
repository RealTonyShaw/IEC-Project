using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasFollowing : MonoBehaviour
{
    // 以哪一个物体为参考系。Canvas会在World Space追踪该物体并达到相对于该物体的固定位置处
    [Header("Settings")]
    public Transform Origin = null;
    //public Vector3 localEulerAngles = new Vector3(0f, 225f, 0f);
    public Vector3 localPosition = new Vector3(1f, 0f, 1f);
    //private Quaternion localRotation;
    //private float yAxis;
    //private Vector3 preTPos;
    //private Quaternion preTRot;
    [Header("Parameters")]
    //public bool enableSync = true;
    //[Range(0f, 1f)]
    //public float syncRate = 0.9f;
    public float normalConst = 25f;
    public float tangentialConst = 5f;
    public AnimationCurve SpeedCurve;
    private Vector3 prePos;

    private void Start()
    {
        prePos = transform.position;
        if (Origin == null)
        {
            Origin = transform.parent;
        }
    }

    private void FixedUpdate()
    {
        SimulateSpeed(Time.fixedDeltaTime);
        Restoring(Time.fixedDeltaTime);
    }

    void SimulateSpeed(float dt)
    {
        Vector3 v = (transform.position - prePos) * 50f;
        v = v.normalized * SpeedCurve.Evaluate(v.magnitude);
        transform.position -= v * dt;
        prePos = transform.position;
    }

    void Restoring(float dt)
    {
        //Origin.TransformDirection
        Vector3 tpos = localPosition;
        Vector3 pos = transform.localPosition;
        // delta
        Vector3 del = tpos - pos;
        Vector3 del_nor = Vector3.Project(del, pos);
        Vector3 del_tan = del - del_nor;
        pos = Vector3.Lerp(pos, pos + del_nor, normalConst * dt);
        pos = Vector3.Lerp(pos, pos + del_tan, tangentialConst * dt);
        transform.localPosition = pos;
    }

    //private void Start()
    //{
    //    localRotation = Quaternion.Euler(localEulerAngles);
    //    yAxis = localEulerAngles.y;
    //    if (Origin == null)
    //    {
    //        Origin = Camera.main.transform;
    //    }
    //    transform.rotation = Origin.rotation * localRotation;
    //    transform.position = Origin.TransformPoint(localPosition);
    //    preTRot = transform.rotation;
    //    preTPos = transform.position;
    //}
    //private void FixedUpdate()
    //{
    //    if (Origin == null)
    //    {
    //        Debug.LogWarning("Canvas Need to have an origin");
    //        return;
    //    }
    //    if (enableSync)
    //        Synchronize(Time.fixedDeltaTime);
    //    UpdatePosition(Time.fixedDeltaTime);
    //    UpdateRotation(Time.fixedDeltaTime);
    //}

    //void Synchronize(float dt)
    //{
    //    Vector3 tpos = Origin.TransformPoint(localPosition);
    //    Quaternion trot = Origin.rotation * localRotation;
    //    transform.position += syncRate * (tpos - preTPos);
    //    Quaternion drot = Quaternion.Inverse(preTRot) * trot;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * drot, syncRate);
    //    preTPos = tpos;
    //    preTRot = trot;
    //}

    //void UpdatePosition(float dt)
    //{
    //    //Origin.TransformDirection
    //    Vector3 tpos = Origin.TransformPoint(localPosition);
    //    Vector3 pos = transform.position;
    //    // normal
    //    Vector3 nor = (tpos - Origin.position).normalized;
    //    // delta
    //    Vector3 del = tpos - pos;
    //    Vector3 del_nor = Vector3.Project(del, nor);
    //    Vector3 del_tan = del - del_nor;
    //    pos = Vector3.Lerp(pos, pos + del_nor, normalConst * dt);
    //    pos = Vector3.Lerp(pos, pos + del_tan, tangentialConst * dt);
    //    transform.position = pos;
    //}

    //void UpdateRotation(float dt)
    //{
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Origin.rotation * localRotation, angularConst * dt);
    //}

}
