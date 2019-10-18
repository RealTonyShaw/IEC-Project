using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MultiGamePanel
{
    bool isListening = false;
    string uName;
    string key;
    public void OnSignInClick(AudioSource audioSource)
    {
        audioSource.Play();
        if (userName.text == null || userName.text.Length < 1)
        {
            return;
        }
        if (password.text == null || password.text.Length < 6)
        {
            return;
        }
        uName = userName.text;
        key = password.text;
        if (ClientLauncher.IsConnected)
        {
            TryLogIn();
        }
        else if (!isListening)
        {
            ClientLauncher.Instance.OnConnected.AddListener(TryLogIn);
            isListening = true;
            ClientLauncher.Instance.Launch();
        }
    }

    void TryLogIn()
    {
        if (isListening)
        {
            ClientLauncher.Instance.OnConnected.RemoveListener(TryLogIn);
            isListening = false;
        }
        DataSync.Login(uName, key);
    }
}