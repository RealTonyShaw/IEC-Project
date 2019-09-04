using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientBase;

public class ClientLauncher : MonoBehaviour
{

    public const uint MAX_CONNECT_TIMES = 10;
    EventHandler eventHandler;

    private static ClientLauncher clientLauncher;
    public static ClientLauncher Instant
    {
        get
        {
            if (clientLauncher == null)
            {
                clientLauncher = new ClientLauncher();
            }
            return clientLauncher;
        }
    }

    private ClientLauncher()
    {

    }

    public void InitClient()
    {
        Client.Instance.Host = "127.0.0.1";
        Client.Instance.port = 3356;
        for (int i = 0; !Client.Instance.isConnect && i < MAX_CONNECT_TIMES; i++)
        {
            Client.Instance.Connect();
        }
    }

    public void Start()
    {
        eventHandler = EventHandler.GetEventHandler();
    }

    public void Update()
    {
        eventHandler.Update();
    }
}
