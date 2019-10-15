using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : PlayerInfoGamePanel
{
    private static SkillPanel instance;
    public static SkillPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillPanel();
            }
            return instance;
        }
    }

    private SkillPanel()
    {

    }
}
