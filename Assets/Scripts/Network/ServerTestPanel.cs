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

    private bool isMatching = false;
    private float mt_timer = 0;

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
            EventHandler.GetEventHandler().AddOnceListener(ProtoName.CancelMatch, OnCancelMatchBack);
        }
        else if (!isMatching)
        {
            EventHandler.GetEventHandler().AddOnceListener(ProtoName.Match, OnMatchBack);
            DataSync.Match();
        }
    }

    public void OnMatchBack(ProtocolBase protocol)
    {
        int start = 0;
        ProtocolBytes proto = (ProtocolBytes)protocol;
        proto.GetNameX(start, ref start);
        int flag = proto.GetByte(start, ref start);
        if (flag == 1)
        {
            isMatching = true;
            Debug.Log("Enter matching list");
        }
    }

    public void OnCancelMatchBack(ProtocolBase protocol)
    {
        int start = 0;
        ProtocolBytes proto = (ProtocolBytes)protocol;
        proto.GetNameX(start, ref start);
        int flag = proto.GetByte(start, ref start);
        if (flag == 1)
        {
            isMatching = false;
            mt_timer = 0;
            Debug.Log("Out of matching list");
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
