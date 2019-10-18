using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RegisterPanel
{
    bool isListening = false;
    string uName;
    string key;

    public void OnConfirmClick()
    {
        if (userName.text == null || userName.text.Length < 1)
        {
            return;
        }
        if (password.text == null || password.text.Length < 6)
        {
            return;
        }
        if (!password.text.Equals(repeatPw.text))
        {
            Debug.Log("Differance password");
            return;
        }
        uName = userName.text;
        key = password.text;
        if (ClientLauncher.IsConnected)
        {
            TryReg();
        }
        else if (!isListening)
        {
            ClientLauncher.Instance.OnConnected.AddListener(TryReg);
            isListening = true;
            ClientLauncher.Instance.Launch();
        }
    }

    void TryReg()
    {
        if (isListening)
        {
            ClientLauncher.Instance.OnConnected.RemoveListener(TryReg);
            isListening = false;
        }
        DataSync.Register(uName, key);
    }
}
