using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerTestPanel : MonoBehaviour
{
    public InputField sendMsg;
    public InputField userName;
    public InputField password;
    public InputField pwConfirm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnRegClicked()
    {
        if (userName.text == null || userName.text.Length < 1)
        {
            return;
        }
        if (password.text == null || password.text.Length < 6)
        {
            return;
        }
        if (!password.text.Equals(pwConfirm.text))
        {
            Debug.Log("Differance password");
            return;
        }
        Debug.Log("Reg");
        DataSync.Register(userName.text, password.text);
    }

    public void OnLoginClicked()
    {
        if (userName.text == null || userName.text.Length < 1)
        {
            return;
        }
        if (password.text == null || password.text.Length < 6)
        {
            return;
        }
        DataSync.Login(userName.text, password.text);
    }

    public void OnSendClicked()
    {
        DataSync.Chatting(sendMsg.text);
    }
}
