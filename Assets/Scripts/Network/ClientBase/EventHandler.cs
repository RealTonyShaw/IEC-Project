using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientBase
{
    /// <summary>
    /// Singleton pattern
    /// Event handler to handle about protocols. Now can only handle fixed listener.
    /// Unimplement : Once listener.
    /// The code seems to handle permanent listeners only.
    /// </summary>
    public class EventHandler
    {
        public delegate void ProtocolProcessor(ProtocolBase protocol);
        private static EventHandler ev;

        public static EventHandler GetEventHandler()
        {
            return ev != null ? ev : (ev = new EventHandler());
        }

        private Dictionary<int, ProtocolProcessor> proDic = new Dictionary<int, ProtocolProcessor>();
        private Dictionary<int, string> proStrDic = new Dictionary<int, string>();

        private Dictionary<ProtoName, ProtocolProcessor> onceListeners = new Dictionary<ProtoName, ProtocolProcessor>();
        private List<ProtoName> listeners = new List<ProtoName>();


        /// <summary>
        /// Add once listener
        /// </summary>
        /// <param name="name">name of the listener</param>
        /// <param name="callBack">the call back function</param>
        public void AddOnceListener(ProtoName _name, ProtocolProcessor callBack)
        {
            if (onceListeners.Keys.Contains(_name))
            {
                onceListeners[_name] += callBack;
            }
            else
            {
                onceListeners.Add(_name, callBack);
            }            
        }

        /// <summary>
        /// Add a listener to the list
        /// </summary>
        /// <param name="name"> the ProtoName</param>
        /// <param name="callBack">the feed back method</param>
        public void AddListenerEvent(ProtoName name, ProtocolProcessor callBack)
        {
            try
            {
                proDic[(int)name] += callBack;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //public void Del

        /// <summary>
        /// Delete a once listener.
        /// </summary>
        /// <param name="proto">the proto name of the listener</param>
        public void DelOnceListener(ProtoName proto)
        {
            if (onceListeners.ContainsKey(proto))
            {
                onceListeners.Remove(proto);
            }            
        }

        private Queue<ProtocolBase> protocols = new Queue<ProtocolBase>();

        private EventHandler()
        {
            InitProtoDic();
        }

        /// <summary>
        /// Init the dic (int, protocolHandlerMethodDelegate)
        /// </summary>
        private void InitProtoDic()
        {
            MsgHandler msg = new MsgHandler();
            MethodInfo[] mi = msg.GetType().GetMethods();
            Dictionary<string, int> proDicStr = SF.GetEnumNameAndNum(typeof(ProtoName));
            //Debug.Log(string.Format("Method info count = {0}, Enum count = {1}", mi.Length, proDicStr.Count));
            foreach (MethodInfo m in mi)
            {
                if (proDicStr.ContainsKey(m.Name))
                {
                    //Debug.Log(m.Name);
                    proDic.Add(proDicStr[m.Name],
                        (ProtocolProcessor)Delegate.CreateDelegate(typeof(ProtocolProcessor), m));
                }
            }            
        }

        /// <summary>
        /// Add a protocol to the waiting list, storing the protocols waiting to handle.
        /// </summary>
        /// <param name="proto">protocol to add</param>
        public void AddProtocol(ProtocolBase proto)
        {
            lock (m_lock)
            {
                protocols.Enqueue(proto);
            }
        }

        private int perFrame = 100;

        /// <summary>
        /// Set the handle frame of protocols.
        /// </summary>
        /// <param name="pfr">The frame that change</param>
        public void SetPerFrame(int pfr)
        {
            perFrame = pfr > 0 ? pfr : perFrame;
        }

        private static readonly object m_lock = new object();

        //private double last = 0;
        /// <summary>
        /// Invoke the method in update, Unity Engine.
        /// </summary>
        /// <param name="deltaTime">Time.DeltaTime</param>
        public void Update()
        {
            for (int i = 0; i < perFrame; i++)
            {
                if (protocols.Count == 0)
                {
                    //Do not use return for declining the delta time between message handle.
                    continue;
                }
                //Lock the protocols for thread safety
                lock (m_lock)
                {
                    Handle(protocols.Dequeue());
                }
            }
        }

        /// <summary>
        /// To handle a protocol.
        /// First handle listener and if not handle once listener.
        /// (First patron and then new costomer)
        /// </summary>
        /// <param name="protocol">The protocol to handle</param>
        private void Handle(ProtocolBase protocol)
        {
            int number = protocol.GetNumber();
            ProtoName pn = (ProtoName)number;
            Debug.Log("Accept" + pn.ToString());
            //Debug.Log("Contain key" + pn.ToString() + " " + proDic.ContainsKey(number));
            //Debug.Log("Contain listener" + pn.ToString() + " " + listeners.Contains(pn));
            if (proDic.ContainsKey(number))//&& listeners.Contains(pn)
            {
                    proDic[number](protocol);
                    Debug.Log("Handle listener" + pn.ToString());                                         
            }
            //Debug.Log("Contain once listener" + pn.ToString() + " " + onceListeners.ContainsKey(pn));
            if (onceListeners.ContainsKey(pn))
            {
                onceListeners[pn](protocol);
                onceListeners.Remove(pn);
                Debug.Log("Handle once listener" + pn.ToString());
            }
        }
    }
}
