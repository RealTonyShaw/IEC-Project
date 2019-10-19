using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameCtrl
{
    public static bool IsLoading { get; private set; }
    AsyncOperation ao;
    public void StartLoadingGameScene()
    {
        loadingPanel.StartLoading();
        Gamef.DelayedExecution(delegate
        {
            IsLoading = true;
            ao = SceneManager.LoadSceneAsync("Game");
            EventMgr.UpdateEvent.AddListener(_checkIsDone);
        }, 0.5f);
    }

    public void StartCreatePlayer(int playerID)
    {
        Debug.Log("Player ID = " + playerID);
        if (GameSceneInfo.Instance == null)
        {
            Debug.Log("Null instance");
        }
        else if (GameSceneInfo.Instance.spawnPoints[playerID] == null)
        {
            Debug.Log("Null spawnPoint");
        }
        Transform t = GameSceneInfo.Instance.spawnPoints[playerID].transform;
        Crosshair.SetState(true);
        if (GameCtrl.IsOnlineGame)
            DataSync.CreateObject(ClientLauncher.PlayerID, UnitName.Player, t.position, t.rotation);
        else
        {
            Gamef.CreateLocalUnit(UnitName.Player, t.position, t.rotation);
        }
    }

    private void _checkIsDone()
    {
        if (ao.isDone)
        {
            EventMgr.UpdateEvent.RemoveListener(_checkIsDone);
            if (IsOnlineGame)
                DataSync.CanControll();
            else
            {
                StartCreatePlayer(0);
            }
            loadingPanel.StopLoading();
            IsLoading = false;
        }
    }
}