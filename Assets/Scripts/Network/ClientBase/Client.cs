using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    public class Client
    {
        public const int MAX_BUFFER_SIZE = 1024;
        private static Client instance;
        public bool isConnect = false;
        public Socket client;

        private ProtocolTestCRC proto = new ProtocolTestCRC();

        private int start = 0;
        private int length = 0;

        public int port = -1;

        private byte[] buffer = new byte[MAX_BUFFER_SIZE];

        private string host;
        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
            }
        }

        public Client Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                {
                    instance = new Client();
                    return instance;
                }
            }
        }
        private Client()
        {

        }
        public void Connect()
        {
            if (host == null)
                return;
            if (client != null)
            {
                Console.WriteLine("There is a connection");
            }
            if (port < 0)
                return;
            try
            {
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint iPEndPoint = new IPEndPoint(ip, port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(iPEndPoint);
                isConnect = true;
                client.BeginReceive(buffer, start, MAX_BUFFER_SIZE - start,
                    SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Invoke this when accept message from client.
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                start += client.EndReceive(ar);
                DataProcessor();
                client.BeginReceive(buffer, start, MAX_BUFFER_SIZE - start,
        SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        byte[] lenBytes = new byte[sizeof(int)];
        /// <summary>
        /// 处理信息，管理接收到的数据
        /// </summary>
        private void DataProcessor()
        {
            //如果小于存储长度的数据长度，则返回
            if (start < sizeof(int))
                return;
            length = BitConverter.ToInt32(buffer, 0);
            //如果没接收完毕返回
            if (start < SF.INT_SIZE + length)
                return;
            ProtocolBase protocol = proto.Decode(buffer, SF.INT_SIZE, length);
            //todo this is to deal with protocol
            bool cor = false;
            if (length > 144)
            {
                CRC16 crc16 = new CRC16(protocol.Encode());
                cor = crc16.IsCorrect();
            }
            else
            {
                CRC8 crc8 = new CRC8(protocol.Encode(), CRC_op.Judge);
                cor = crc8.IsCorrect();
            }

            if (cor)
            {
                //Add handler and handle.
                EventHandler.GetEventHandler().AddProtocol(protocol);
            }

            //Operations for protocol
            int count = start - SF.INT_SIZE - length;
            Array.Copy(buffer, start, buffer, 0, count);
            if (count > 0)
                DataProcessor();
            return;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Reconnect()
        {
            if (isConnect)
            {
                return;
            }
        }

        /// <summary>
        /// Send msg to server
        /// </summary>
        public void Send(ProtocolBase protocol)
        {
            if (!isConnect)
                return;
            byte[] sendBuffer = protocol.Encode();
            client.Send(sendBuffer);
        }
    }
}
