using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit组件视同于单位。
/// 挂载Unit组件的游戏物体即为单位。
/// Unit负责注册及注销当前单位，在Start中注册单位，同时在OnDestroy中注销单位。
/// 其中，注册和注销分别指将单位加入单位池并且分配ID和将单位从单位池中移除。
/// Unit负责处理与单位的生命周期相关的事宜。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public partial class Unit : MonoBehaviour
{
    public UnitCanvasController canvas;
    [HideInInspector]
    public Rigidbody rigbody;
    public Transform SpawnTransform;
    public UnitName unitName;
    public AnimatorController animatorController;
    public float DestroyDelay = 3f;
    public UnitAttributes attributes;
    //public Canvas unitCanvas;
    //public Transform unitCamera;
    [SerializeField]
    private float runtimeAccuracy;
    // 射击精确度
    public float RuntimeAccuracy
    {
        get
        {
            return runtimeAccuracy;
        }

        set
        {
            // 实时精确度应当介于最小值 10 到最大值 accuracy 之间。
            // accuracy 是技能数据，指技能最大的精确度。
            runtimeAccuracy = Mathf.Clamp(value, skillTable.CurrentSkill.Data.MinAccuracy, skillTable.CurrentSkill.Data.Accuracy);
        }
    }
    // 技能表
    ISkillTable skillTable = new SkillTable();
    public ISkillTable SkillTable => skillTable;

    // 单位私有事件
    public readonly MyActionEvent StartCastingEvnt = new MyActionEvent();
    public readonly MyActionEvent StopCastingEvnt = new MyActionEvent();
    public readonly MyActionEvent DeathEvnt = new MyActionEvent();
    public readonly MyActionEvent TakeDmgEvnt = new MyActionEvent();
    public readonly MyActionEvent<int> SwitchSkillEvnt = new MyActionEvent<int>();
    public readonly MyActionEvent OnHit = new MyActionEvent();

    [Header("Network Synchronization")]
    public bool IsLocal = true;
    public bool RecvSyncMovement = true;
    public bool RecvSyncUnitState = true;
    public bool RecvSyncPlayerCastingState = false;
    public bool RecvSyncPlayerInput = false;

    public long lastSyncUnitStateInstant = 0;
    // 位置同步
    public ISyncMovement SyncMovement { get; private set; }
    // 单位状态同步
    public ISyncUnitState SyncUnitState { get; private set; }
    // 玩家施法同步
    public ISyncPlayerCastingState SyncPlayerCastingState { get; private set; }
    // 玩家输入同步
    public ISyncPlayerInput SyncPlayerInput { get; private set; }

    #region 生命周期
    private void Awake()
    {
        rigbody = GetComponent<Rigidbody>();
        if (SpawnTransform == null)
        {
            SpawnTransform = transform;
        }
        //加入监听SP变化事件
        EventMgr.SPChangeEvent.AddListener(SPEvent);

        if (GameCtrl.IsOnlineGame)
        {
            if (RecvSyncMovement)
            {
                SyncMovement = new SyncMovement();
                SyncMovement.Init(this);
            }
            if (RecvSyncUnitState)
            {
                SyncUnitState = new SyncUnitState();
                SyncUnitState.Init(this);
            }
            if (RecvSyncPlayerCastingState)
            {
                SyncPlayerCastingState = new SyncPlayerCasting();
                SyncPlayerCastingState.Init(this);
            }
            if (RecvSyncPlayerInput)
            {
                SyncPlayerInput = new SyncPlayerInput();
                SyncPlayerInput.Init(this);
            }
        }

    }

    private void Start()
    {
        if (gameObject.layer != Layer.Unit)
        {
            Debug.LogError(string.Format("Unit {0} is not in Unit layer.", gameObject.name));
        }
        if (GameCtrl.IsOnlineGame && attributes.ID == -1)
        {
            Debug.LogError("ID not set!!!!");
        }
        if (!GameCtrl.IsOnlineGame)
        {
            InitAttributes();
        }
        MyStart();
    }

    protected virtual void MyStart()
    {

    }

    private bool isInitAttr = false;
    public void InitAttributes()
    {
        if (isInitAttr)
            return;
        isInitAttr = true;
        //注册单位
        lock (GameDB.unitPool)
            attributes.ID = Gamef.UnitBirth(this);

        attributes.Init(this);
        // 如果该单位是施法单位，则初始化技能表
        if (attributes.data.IsCaster)
            skillTable.Init(this);
    }
    /// <summary>
    /// 外部可以通过该接口对单位进行初始化。
    /// </summary>
    /// <param name="ID">单位ID</param>
    public void InitAttributes(int ID)
    {
        if (isInitAttr)
            return;
        isInitAttr = true;
        //注册单位
        lock (GameDB.unitPool)
            attributes.ID = Gamef.UnitBirth(this, ID);

        attributes.Init(this);
        // 如果该单位是施法单位，则初始化技能表
        if (attributes.data.IsCaster)
            skillTable.Init(this);
    }

    protected virtual void Update()
    {
        if (!attributes.isAlive)
            return;
        //回复 护盾值
        attributes.SheildPoint += attributes.SPRegenerationRate.Value * Time.deltaTime;

        if (attributes.data.IsCaster)
        {
            //回复 魔法值 和 当前技能冷却（应当是全体技能冷却，同时技能初始精确度未设置）
            attributes.ManaPoint.Value += attributes.MPRegenerationRate.Value * Time.deltaTime;
            skillTable.CurrentSkill.AccuracyCooldown(Time.deltaTime);
        }

        //触发buff效果
        BuffEvent?.Invoke();

        if (GameCtrl.IsOnlineGame && IsLocal)
        {
            if (Gamef.SystemTimeInMillisecond - lastSyncUnitStateInstant >= GameDB.SYNC_TRANSFORM_INTERVAL)
            {
                Debug.Log("Send Hp = " + attributes.SheildPoint + ", Mp = " + attributes.ManaPoint.Value);
                lastSyncUnitStateInstant = Gamef.SystemTimeInMillisecond;
                DataSync.SyncHP(this, lastSyncUnitStateInstant, attributes.SheildPoint);
                DataSync.SyncMP(this, lastSyncUnitStateInstant, attributes.ManaPoint.Value);
            }
        }

        ////单位画布面对摄像机
        //if (unitCanvas != null)
        //    unitCanvas.transform.forward = unitCamera.position - unitCanvas.transform.position;
    }

    private void FixedUpdate()
    {
        // 执行位置同步
        SyncMovement?.Update(Time.fixedDeltaTime);
    }

    #endregion

    #region 生命值
    /// <summary>
    /// 单位受伤
    /// </summary>
    /// <param name="amount">伤害值</param>
    public virtual void TakeDamage(float amount)
    {
        OnHit.Trigger();
        if (!IsLocal || !attributes.isAlive)
            return;
        if (amount < 0)
        {
            Debug.Log("这已经不是挠痒痒的伤害了");
        }
        //减少护盾值
        attributes.SheildPoint -= amount;
        TakeDmgEvnt.Trigger();
    }

    /// <summary>
    /// 单位回复护盾护盾
    /// </summary>
    /// <param name="amount">回复量</param>
    public virtual void BeHealed(float amount)
    {
        if (!IsLocal || !attributes.isAlive)
            return;
        if (amount < 0)
        {
            Debug.Log("你真是口毒奶");
        }
        //回复护盾值
        attributes.SheildPoint += amount;
    }

    bool sendDeathRequest = false;
    object deathRequestMutex = new object();
    /// <summary>
    /// 护盾值事件，包括因为护盾损失和恢复在内的一切效果的显现等。
    /// 考虑以后加入Death类，作为静态函数，统一处理。
    /// </summary>
    /// <param name="info"></param>
    private void SPEvent(EventMgr.SPChangeEventInfo info)
    {
        //只负责实现当前挂载单位的效果
        if (info.Unit != this)
        {
            return;
        }

        if (GameCtrl.IsOnlineGame && IsLocal)
        {
            lock (deathRequestMutex)
            {
                if (!sendDeathRequest)
                    if (IsLocal && info.CurrentValue <= 1e-3f)
                    {
                        sendDeathRequest = true;
                        Death();
                    }
            }
        }
        else if (!GameCtrl.IsOnlineGame)
        {
            //SP过低，死亡
            if (info.CurrentValue <= 1e-3f)
                Death();
        }

    }

    public virtual void Death()
    {
        if (!attributes.isAlive)
            return;
        attributes.isAlive = false;
        if (GameCtrl.PlayerUnit == this)
        {
            DeathPanel.Instance.BeginDeath();
        }
        if (GameCtrl.IsOnlineGame && IsLocal)
        {
            DataSync.SyncHP(this, Gamef.SystemTimeInMillisecond, 0f);
            DataSync.DestroyObj((byte)attributes.ID);
        }
        if (attributes.data.IsCaster)
            skillTable.CurrentCell.Stop();
        if (attributes.SheildPoint > 0f)
        {
            attributes.SheildPoint = 0f;
        }
        //清空所有buff
        while (buffs.Count > 0)
            LogOffBuff(buffs[0]);
        Debug.Log(gameObject.name + " has died.");
        DeathEvnt.Trigger();
        //注销单位
        lock (GameDB.unitPool)
            Gamef.UnitClear(this);
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(DestroyDelay);
        Gamef.Destroy(gameObject);
        if (GameCtrl.IsOnlineGame && IsLocal && attributes.name == UnitName.Player)
        {
            Transform t = GameSceneInfo.Instance.spawnPoints[ClientLauncher.PlayerID].transform;
            DataSync.CreateObject(ClientLauncher.PlayerID, UnitName.Player, t.position, t.rotation);
        }
        else if (!GameCtrl.IsOnlineGame)
        {
            Transform t = GameSceneInfo.Instance.spawnPoints[0].transform;
            Gamef.CreateLocalUnit(UnitName.Player, t.position, t.rotation);
        }
    }

    #endregion
}
