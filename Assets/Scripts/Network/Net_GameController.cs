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

    private void _checkIsDone()
    {
        if (ao.isDone)
        {
            DataSync.CanControll();
            EventMgr.UpdateEvent.RemoveListener(_checkIsDone);
        }
    }
}