using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Valve.VR;

public partial class GameCtrl : MonoBehaviour
{
    #region 单例
    /// <summary>
    /// 游戏控制器单例
    /// </summary>
    public static GameCtrl Instance { get; private set; }
    /// <summary>
    /// 游戏控制组件是否初始化完成。在Controller的Start执行后为真。
    /// </summary>
    public static bool isInit = false;
    #endregion

    #region 实时公有信息
    public LogoPanel logoPanel;
    public LoadingPanel loadingPanel;
    public static bool CursorOnGUI = false;

    //private UnitInfo _mainChara;
    private static Unit _playerUnit = null;
    public static EventPublisher<Unit> PlayerUnitChangeEvent = new EventPublisher<Unit>();
    public static Unit PlayerUnit
    {
        get
        {
            //if (_playerUnit == null)
            //{
            //    _playerUnit = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Unit>();
            //    PlayerUnitChangeEvent.OnTrigger(_playerUnit);
            //}
            return _playerUnit;
        }

        set
        {
            _playerUnit = value;
            PlayerUnitChangeEvent.OnTrigger(_playerUnit);
        }
    }
    public static bool IsOnlineGame = false;
    //public static bool IsVR = true;
    //public bool Is_VR = true;
    //public bool Is_Online_Game = false;

    public Transform PlayerCamera
    {
        get; set;
    }
    #endregion

    //public SteamVR_Action_Vibration hapticSignal;
    public void StartSingleGame()
    {
        StartLoadingGameScene();
    }



    /// <summary>
    /// 延迟执行动作。
    /// </summary>
    /// <param name="action">动作，即延迟执行的空参数空返回值的方法</param>
    /// <param name="time">延迟时间</param>
    public void DelayedExecution(Action action, float time)
    {
        StartCoroutine(m_delayedExecution(action, time));
    }
    IEnumerator m_delayedExecution(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public bool BuildDataPath = false;
    #region 生命周期
    private void Awake()
    {
        Instance = this;
        EventMgr.initEvent.OnAwake();
        EventMgr.UpdateEvent.AddListener(InputMgr.CheckHotKey);
    }

    private void Start()
    {
        EventMgr.initEvent.OnStart();
        DontDestroyOnLoad(gameObject);
        GameObjectCache.Init();
        isInit = true;//初始化完毕

        if (BuildDataPath)
            Build();

        ////加载游戏场景
        //if (check)
        //{
        //    SceneManager.LoadSceneAsync(gameScene);
        //}
        //else
        //    SceneManager.LoadSceneAsync(GameDB.MyScene.GameScene);

        //SceneManager.LoadSceneAsync("Demo_Exterior");
    }

    private void Update()
    {
        CheckInputForSkillTable();

        EventMgr.UpdateEvent.OnTrigger();

        UpdateMP_HP_UI();
        UpdateCrosshair();
    }

    void UpdateMP_HP_UI()
    {
        if (PlayerUnit != null)
        {
            UnitAttributes attr = PlayerUnit.attributes;
            Gamef.SetHP_UI_Rate(attr.SheildPoint / attr.MaxShieldPoint);
            Gamef.SetMP_UI_Rate(attr.ManaPoint.Value / attr.MaxManaPoint.Value);
        }
    }

    void UpdateCrosshair()
    {
        if (PlayerUnit != null)
        {
            if (PlayerUnit.RuntimeAccuracy > 9.99f)
                Crosshair.Instance.SetAccuracy(PlayerUnit.RuntimeAccuracy);
        }
    }
    #endregion

    private void Build()
    {
        // 建立单位数据路径
        string[] paths = GameDB.Instance.unitDataPath.paths = new string[(int)UnitName.MaxIndex];
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = "Unit/" + ((UnitName)i).ToString() + "Data";
        }
        // 建立技能数据路径
        paths = GameDB.Instance.skillDataPath.paths = new string[GameDB.MAX_SKILL_INDEX];
        foreach (SkillName name in Enum.GetValues(typeof(SkillName)))
        {
            int i = (int)name;
            paths[i] = "Skill/" + name + "Data";
        }
        // 建立技能类的路径
        SkillFactory.Init();
        //// 建立Prefab路径
        //Array enums = Enum.GetValues(typeof(PrefabName));
        //paths = GameDB.Instance.prefabPath.paths = new string[enums.Length];
        //foreach (PrefabName name in enums)
        //{
        //    int i = (int)name;
        //    paths[i] = name.ToString();
        //}

    }
}

/// <summary>
/// 单位的全面信息
/// </summary>
public class UnitInfo
{
    public Unit UnitCtrl { get; private set; }
    public GameObject Obj { get; private set; }
    public Transform Transform { get; private set; }
    /// <summary>
    /// 构造单位全面信息
    /// </summary>
    /// <param name="unitCtrl">单位控制组件</param>
    public UnitInfo(Unit unitCtrl)
    {
        UnitCtrl = unitCtrl;
        Obj = unitCtrl.gameObject;
        Transform = Obj.transform;
    }
}

/// <summary>
/// 护甲类型
/// </summary>
public enum ArmorType
{
    //无敌的
    invincible = 0,
    //

    //

    //
}

/// <summary>
/// 伤害类型
/// </summary>
public enum DamageType
{
    unset,
}