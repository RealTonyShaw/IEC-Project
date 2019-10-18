using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public GameObject Shield;
    public ShieldColorController shieldColorController;

    public override void Death()
    {
        base.Death();
        Shield.SetActive(false);
    }

    float lastHP = 0f;
    protected override void Update()
    {
        base.Update();
        if (lastHP > attributes.SheildPoint + 1e-2f)
        {
            shieldColorController.Trigger();
        }
        lastHP = attributes.SheildPoint;
    }
}
