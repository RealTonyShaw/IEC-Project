using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldColorController : MonoBehaviour
{
    public bool trigger = false;
    public Color DefaultColor;
    public Color CurrentColor;
    public Color TargetColor;
    public Color TriggerColor = Color.red;
    public float UpdateSpeed = 255f;
    public float TriggerTime = 0.1f;
    private float stopTime;

    private Material magicMaterial;
    private void Start()
    {
        CurrentColor = DefaultColor;
        TargetColor = DefaultColor;
        magicMaterial = GetComponent<Renderer>().material;
    }


    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            Trigger();
        }
        Vector4 cur = retrieveV4(CurrentColor);
        Vector4 tar = retrieveV4(TargetColor);
        Vector4 diff = tar - cur;
        float dis = UpdateSpeed * Time.deltaTime;
        if (diff.sqrMagnitude > dis * dis)
        {
            cur += diff.normalized * dis;
        }
        else
        {
            cur = tar;
        }
        adjustColor(cur, ref CurrentColor);
        magicMaterial.SetColor("_Color", CurrentColor);
    }

    public void Trigger()
    {
        stopTime = Time.time + TriggerTime - 1e-3f;
        TargetColor = TriggerColor;
        Gamef.DelayedExecution(delegate
        {
            if (Time.time >= stopTime)
                TargetColor = DefaultColor;
        }, TriggerTime);
    }

    Vector4 retrieveV4(Color c)
    {
        return new Vector4(c.r, c.g, c.b, c.a);
    }

    void adjustColor(Vector4 v, ref Color c)
    {
        c.r = v.x;
        c.g = v.y;
        c.b = v.z;
        c.a = v.w;
    }
}
