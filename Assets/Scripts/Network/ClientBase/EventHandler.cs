using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        private List<ProtoName> onceListeners = new List<ProtoName>();
        private List<ProtoName> listeners = new List<ProtoName>();


        /// <summary>
        /// Add once listener
        /// </summary>
        /// <param name="name">name of the listener</param>
        /// <param name="callBack">the call back function</param>
        public void AddOnceListener(ProtoName name)
        {
            onceListeners.Add(name);
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
            onceListeners.Remove(proto);
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
            foreach (MethodInfo m in mi)
            {
                if (proDicStr.ContainsKey(m.Name))
                {
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
            lock (protocols)
            {
                protocols.Enqueue(proto);
            }
        }

        private int perFrame = 10;

        /// <summary>
        /// Set the handle frame of protocols.
        /// </summary>
        /// <param name="pfr">The frame that change</param>
        public void SetPerFrame(int pfr)
        {
            perFrame = pfr > 0 ? pfr : perFrame;
        }

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
                lock (protocols)
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
            if (proDic.ContainsKey(number))
            {
                if (listeners.Contains(pn)) { }
                else if (onceListeners.Contains(pn)) { onceListeners.Remove(pn); }                                
                else{ return;}
                proDic[number](protocol);
            }
        }
    }
}
