using ClientBase;
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
    public InputField ipAdr;
    public InputField port;
    private CanvasGroup canvasGroup;
    public GameObject loginAndregPanel;
    private CanvasGroup larPaCag;

    public GameObject launcherHost;
    private ClientLauncher launcher;

    private float mt_timer = 0;
    private bool isMatching = false;

    // Start is called before the first frame update
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        larPaCag = loginAndregPanel.GetComponent<CanvasGroup>();
        launcher = launcherHost.GetComponent<ClientLauncher>();
    }

    private void Update()
    {
        if (isMatching)
        {
            mt_timer += Time.deltaTime;
        }
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

    public void OnMatchClick()
    {
        if (isMatching && mt_timer > 3)
        {
            DataSync.CancelMatch();
            isMatching = false;
            mt_timer = 0;
        }
        else if (!isMatching)
        {
            DataSync.Match();
            isMatching = true;
        }
    }

    public void OnConnectClicked()
    {
        if (Client.Instance.isConnect) { return; }
        launcher.Connect(ipAdr.text, port.text);
    }

    public void OnSendClicked()
    {
        DataSync.Chatting(sendMsg.text);
    }
}
