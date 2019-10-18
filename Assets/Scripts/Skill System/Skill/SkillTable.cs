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
    public int CurrentIndex => currentSkillNum;

    public void Init(Unit caster)
    {
        // Test Needed
        this.caster = caster;

        for (int i = 0; i < 4; i++)
        {
            SkillCells[i] = new SkillCell();
            SkillCells[i].Init(caster);
            // 设置初始技能
            ISkill tmpSkill = SkillFactory.CreateSkill(caster.attributes.data.skills[i]);
            if (tmpSkill != null)
            {
                SkillCells[i].CurrentSkill = tmpSkill;
            }
        }
    }

    public void SwitchCell(int cellIndex)
    {
        if (currentSkillNum == cellIndex)
        {
            return;
        }
        // 如果切换前的技能还在施放，应该立即停止该技能
        SkillCells[currentSkillNum - 1].Stop();
        currentSkillNum = cellIndex;
        SetUnitInitAccuracy(SkillCells[currentSkillNum - 1].CurrentSkill.Data.Accuracy);
        caster.SwitchSkillEvnt.Trigger(cellIndex);
    }

    private void SetUnitInitAccuracy(float accuracy)
    {
        caster.RuntimeAccuracy = accuracy * GameDB.INITIAL_ACCURACY;
    }
}