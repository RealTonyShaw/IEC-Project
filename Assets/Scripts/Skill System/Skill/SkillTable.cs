using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTable : ISkillTable
{
    // 第 0，1，2 格技能为可切换技能，3 格技能为持续型技能
    public ISkillCell[] SkillCells = new SkillCell[4];
    private int currentSkillNum = 1;
    // Test Needed
    private Unit caster;

    public ISkill CurrentSkill => SkillCells[currentSkillNum - 1].CurrentSkill;
    public ISkillCell CurrentCell => SkillCells[currentSkillNum - 1];

    public void Init(Unit caster)
    {
        // Test Needed
        this.caster = caster;

        for (int i = 0; i < 4; i++)
        {
            SkillCells[i] = new SkillCell();
            SkillCells[i].Init(caster);
            // 设置初始技能
            ISkill tmpSkill = ConcreteSkillFactory.CreateSkill(caster.attributes.data.skills[i]);
            if (tmpSkill != null)
            {
                SkillCells[i].CurrentSkill = tmpSkill;
            }
        }
    }

    public void SwitchCell(int cellIndex)
    {
        // 判定切换前技能是否与切换后技能相同，如果不同则为 true，即可以重置精确度
        bool flag = false;
        switch (cellIndex)
        {
            case 1:
                // 如果切换前的技能还在施放，应该立即停止该技能
                if (currentSkillNum != 1)
                {
                    SkillCells[currentSkillNum - 1].ForceToStopCasting();
                    flag = true;
                }
                currentSkillNum = 1;
                if (flag)
                    SetUnitInitAccuracy(SkillCells[currentSkillNum - 1].CurrentSkill.Data.Accuracy);
                flag = false;
                Debug.Log("切换至技能 1");
                break;
            case 2:
                if (currentSkillNum != 2)
                {
                    SkillCells[currentSkillNum - 1].ForceToStopCasting();
                    flag = true;
                }
                currentSkillNum = 2;
                if (flag)
                    SetUnitInitAccuracy(SkillCells[currentSkillNum - 1].CurrentSkill.Data.Accuracy);
                flag = false;
                Debug.Log("切换至技能 2");
                break;
            case 3:
                if (currentSkillNum != 3)
                {
                    SkillCells[currentSkillNum - 1].ForceToStopCasting();
                    flag = true;
                }
                currentSkillNum = 3;
                if (flag)
                    SetUnitInitAccuracy(SkillCells[currentSkillNum - 1].CurrentSkill.Data.Accuracy);
                flag = false;
                Debug.Log("切换至技能 3");
                break;
        }
    }

    private void SetUnitInitAccuracy(float accuracy)
    {
        caster.RuntimeAccuracy = accuracy * GameDB.INITIAL_ACCURACY;
    }
}