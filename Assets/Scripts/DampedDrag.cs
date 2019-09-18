using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// 产生阻尼，包括运动阻尼和角阻尼
/// </summary>
public class DampedDrag : MonoBehaviour
{
    public bool EnableHorizontalDampedDrag = false;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (EnableHorizontalDampedDrag)
        {
            Vector3 v = rb.velocity;
            rb.velocity -= (Vector3.Project(v, transform.forward) * GameDB.DAMPED_CONST + Vector3.Project(v, transform.right) * GameDB.DAMPED_HORIZON_CONST) * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity -= rb.velocity * GameDB.DAMPED_CONST * Time.fixedDeltaTime;
        }
    }
}
