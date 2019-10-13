using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameCtrl
{
    AsyncOperation ao;
    public void StartLoadingGameScene()
    {
        ao = SceneManager.LoadSceneAsync(gameScene);
        EventMgr.UpdateEvent.AddListener(_checkIsDone);
    }

    public void StartCreatePlayer(int playerID)
    {
        Transform t = GameSceneInfo.Instance.spawnPoints[playerID].transform;

        DataSync.CreateObject(UnitName.Player, t.position, t.rotation);
    }

    private void _checkIsDone()
    {
        if (ao.isDone)
        {
            DataSync.CanControll();
            EventMgr.UpdateEvent.RemoveListener(_checkIsDone);
        }
    }
}