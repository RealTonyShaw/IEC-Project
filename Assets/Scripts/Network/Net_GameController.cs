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
        if (IsLoading)
            return;
        IsLoading = true;
        loadingPanel.StartLoading();
        Gamef.DelayedExecution(delegate
        {
            ao = SceneManager.LoadSceneAsync("Game");
            EventMgr.UpdateEvent.AddListener(afterLoadGame);
        }, 0.5f);
    }

    public void Back2Menu()
    {
        if (IsLoading)
            return;
        IsLoading = true;
        Crosshair.SetState(false);
        loadingPanel.StartLoading();
        Gamef.DelayedExecution(delegate
        {
            ao = SceneManager.LoadSceneAsync("Menu Scene");
            EventMgr.UpdateEvent.AddListener(afterLoadMenu);
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
        if (GameCtrl.IsOnlineGame)
            DataSync.CreateObject(ClientLauncher.PlayerID, UnitName.Player, t.position, t.rotation);
        else
        {
            Gamef.CreateLocalUnit(UnitName.Player, t.position, t.rotation);
        }
    }

    private void afterLoadGame()
    {
        if (ao.isDone)
        {
            EventMgr.UpdateEvent.RemoveListener(afterLoadGame);
            if (IsOnlineGame)
                DataSync.CanControll();
            else
            {
                StartCreatePlayer(0);
                Gamef.DelayedExecution(delegate { Crosshair.SetState(false); }, 0.7f);
                Gamef.DelayedExecution(loadingPanel.StopLoading, 0.2f);
            }
            IsLoading = false;
        }
    }


    private void afterLoadMenu()
    {
        if (ao.isDone)
        {
            EventMgr.UpdateEvent.RemoveListener(afterLoadMenu);
            if (IsOnlineGame)
                ClientLauncher.Instance.Disconnect();
            Gamef.DelayedExecution(loadingPanel.StopLoading, 0.2f);
            GameDB.unitPool.Clear();
            GameDB.missilePool.Clear();
            IsLoading = false;
        }
    }
}