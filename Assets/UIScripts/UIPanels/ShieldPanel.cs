using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPanel : PlayerInfoGamePanel
{
    private static ShieldPanel instance;
    public static ShieldPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ShieldPanel();
            }
            return instance;
        }
    }

    private ShieldPanel()
    {

    }
}
