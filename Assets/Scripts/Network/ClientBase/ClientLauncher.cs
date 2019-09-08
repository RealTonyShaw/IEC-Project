using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientBase;
using System.Diagnostics;
using System;

public class ClientLauncher : MonoBehaviour
{

    public const uint MAX_CONNECT_TIMES = 10;
    private ClientBase.EventHandler eventHandler;
    private Client client;
    private TimeMgr timeMgr;
    private int ping = 0;
    private long lastSend = 0;
    private static ClientLauncher clientLauncher;
    public static ClientLauncher Instant
    {
        get
        {
            return clientLauncher;
        }
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

    public void Awake()
    {
        clientLauncher = this;
    }

    public void Start()
    {
        eventHandler = ClientBase.EventHandler.GetEventHandler();
        client = Client.Instance;
    }

    public void Update()
    {
        eventHandler.Update();
    }

    /// <summary>
    /// When ping server invoke this.
    /// </summary>
    public void Ping()
    {
        lastSend = timeMgr == null ? 0 : timeMgr.GetTime();
    }
    /// <summary>
    /// When ping back call this methods to calculate pings.
    /// </summary>
    public void PingBack()
    {
        ping = (int)((timeMgr == null ? 0 : timeMgr.GetTime()) - lastSend) >> 1;
    }

    /// <summary>
    /// To check the client time accord to Server time.
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <param name="ticks"></param>
    public void TimeCheck(long delta)
    {
        timeMgr.TimeCheck(delta);
    }

    /// <summary>
    /// Get the ticks of client time.
    /// </summary>
    /// <returns>Instant ticks</returns>
    public long GetTime()
    {        
        return timeMgr == null ? 0 : timeMgr.GetTime();
    }


    public class TimeMgr
    {
        private bool isStart = false;
        //private int instant = 0;
        private long bias;
        private Stopwatch stopwatch = new Stopwatch();

        public TimeMgr() { }

        public bool StartTimer()
        {
            if (isStart) { return false; }
            stopwatch.Start();
            isStart = true;
            return isStart;
        }

        public long GetTime()
        {
            if (!isStart) { return 0; }
            return stopwatch.ElapsedMilliseconds + bias;
        }

        private long DTO = ((new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 10000;
        
        /// <summary>
        /// To check the timer.
        /// </summary>
        /// <param name="delta">server timer ticks - ticks, must a negetive number</param>
        public void TimeCheck(long delta)
        {
            if (!isStart)
            {
                return;
            }
            long tick = DateTime.UtcNow.Ticks / 10000 - DTO;
            bias = tick + delta - stopwatch.ElapsedMilliseconds;
        }
    }
}
