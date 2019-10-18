using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBurstfireSkill : ISkill, ISkillCastInstant
{
    public SkillData Data { get; protected set; }
    // 施法者
    protected Unit Caster { get; private set; }
    // 技能特效产生位置
    protected Transform SpawnTransform { get; private set; }

    /// <summary>
    /// 开始释放连射型技能
    /// </summary>
    /// <returns>发出的投掷物</returns>
    protected abstract Missile Shoot();

    /// <summary>
    /// 根据具体技能，读取对应的技能数据。
    /// </summary>
    protected abstract void LoadData();

    public void Init(Unit caster)
    {
        random = new System.Random((int)(Time.time * 1000f));
        this.Caster = caster;
        this.SpawnTransform = caster.SpawnTransform;
        LoadData();
    }

    public void Trigger()
    {
        Debug.Log(string.Format("ID {0} burstfire skill start at {1}", Caster.attributes.ID, Gamef.SystemTimeInMillisecond));
        //if (useGivenSeed)
        //{
        //    if (isFirst)
        //    {
        //        isFirst = false;
        //        Missile missile = Shoot();
        //        if (missile == null)
        //            return;
        //        float dx = Data.Speed * (Gamef.SystemTimeInMillisecond - instant) / 1000f;
        //        missile.transform.Translate(Vector3.forward * dx);
        //        return;
        //    }
        //}
        Shoot();
    }

    public abstract void AccuracyCooldown(float dt);

    protected System.Random random;
    private long instant;
    //private bool useGivenSeed = false;
    //private bool isFirst = false;
    public void SetInstant(long instant)
    {
        this.instant = instant;
        random = new System.Random((int)instant);
        //useGivenSeed = true;
        //isFirst = true;
    }
}
