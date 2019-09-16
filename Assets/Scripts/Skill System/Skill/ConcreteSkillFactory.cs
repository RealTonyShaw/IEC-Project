using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillFactory
{
    private static Dictionary<SkillName, Type> skillDict = new Dictionary<SkillName, Type>();
    private static SkillClassIndex classIndex;
    /// <summary>
    /// 通过技能名创建对应的技能类的实例
    /// </summary>
    /// <param name="name">技能名</param>
    /// <returns>对应的技能类的实例</returns>
    public static ISkill CreateSkill(SkillName name)
    {
        if (!isInit)
        {
            Init();
        }
        int index = (int)name;
        if (classIndex.SkillClasses[index] != null)
        {
            Type type = classIndex.SkillClasses[index];
            return (ISkill)Activator.CreateInstance(type);
        }
        UnityEngine.Debug.Log("No class for skill named :" + name.ToString());
        return null;
    }


    private static bool isInit = false;
    private static readonly object mutex = new object();
    public static void Init()
    {
        lock (mutex)
        {
            if (isInit)
            {
                return;
            }
            classIndex = Resources.Load<SkillClassIndex>("Skill/Skill Class Index");
            int maxIndex = -1;
            foreach (SkillName name in Enum.GetValues(typeof(SkillName)))
            {
                if (maxIndex < (int)name)
                {
                    maxIndex = (int)name;
                }
                Type type = Type.GetType("Skill_" + name.ToString());
                if (type == null)
                {
                    UnityEngine.Debug.Log("No class for skill named :" + name.ToString());
                    continue;
                }
                skillDict.Add(name, type);
            }
            classIndex.SkillClasses = new Type[maxIndex + 1];
            foreach (var item in skillDict)
            {
                classIndex.SkillClasses[(int)item.Key] = item.Value;
            }

            isInit = true;
        }
    }
}
