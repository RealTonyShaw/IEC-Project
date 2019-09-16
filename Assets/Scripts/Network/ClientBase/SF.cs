using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    public partial class SF
    {
        #region ServerBase
        /// <summary>
        /// 获得协议头部
        /// </summary>
        /// <param name="protoName"></param>
        /// <returns></returns>
        public static ProtocolBytes GetProtocolHead(ProtoName protoName)
        {
            ProtocolBytes proto = new ProtocolBytes();
            proto.AddShort((byte)protoName);
            return proto;
        }

        ///// <summary>
        ///// 向指定的Player发送协议
        ///// </summary>
        ///// <param name="player"></param>
        ///// <param name="protocolBase"></param>
        //public static void SendToPlayer(Player player, ProtocolBase protocolBase)
        //{
        //    if (ServNet.instance != null)
        //        ServNet.instance.SendToPlayer(player, protocolBase);
        //    else NullServNet();
        //    return;
        //}

        #region CRC
        public static bool CRC8(byte[] data)
        {
            CRC8 j = new CRC8(data, CRC_op.Judge);
            return !j.IsError();
        }
        public static bool CRC8(ProtocolBytes protocol)
        {
            return CRC8(protocol.bytes);
        }
        public static bool CRC16(byte[] data)
        {
            CRC16 j = new CRC16(data);
            return j.IsCorrect();
        }
        public static bool CRC16(ProtocolBytes protocol)
        {
            return CRC16(protocol.bytes);
        }
        public const int INT_SIZE = sizeof(int);
        public const int DOUBLE_SIZE = sizeof(double);
        public const int FLOAT_SIZE = sizeof(float);
        public const int SHORT_SIZE = sizeof(short);
        #endregion

        #region Protocol Register
        /// <summary>
        /// To parse a protocol dictionary. 
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <param name="name">the name of the file</param>
        /// <returns></returns>
        public static Dictionary<string, int> ParseProtocolDic(string path, string name)
        {
            try
            {
                using (FileStream fs = new FileStream(path + name, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return bf.Deserialize(fs) as Dictionary<string, int>;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// To reverse a proto dic for convert use.
        /// </summary>
        /// <param name="dic">The protocol dic</param>
        /// <returns>The dic that from int to proto name</returns>
        public static Dictionary<string, int> ReverseProtocolDic(Dictionary<int, string> dic)
        {
            Dictionary<string, int> resD = new Dictionary<string, int>();
            foreach (int i in dic.Keys)
            {
                if (resD.Keys.Contains(dic[i]))
                {
                    return null;
                }
                resD.Add(dic[i], i);
            }
            return resD;
        }        
        #endregion



        #endregion

        #region Util
        #region Enum tool
        public static Dictionary<int, string> GetEnumNumAndName(Type t)
        {
            if (!t.IsEnum)
            {
                return null;
            }
            Dictionary<int, string> dic = new Dictionary<int, string>();
            Array ar = t.GetEnumValues();
            string tmp;
            foreach (int i in ar)
            {
                tmp = t.GetEnumName(i);
                if (tmp != null)
                {
                    dic.Add(i, tmp);
                }
            }
            return dic;
        }
        public static Dictionary<string, int> GetEnumNameAndNum(Type t)
        {
            if (!t.IsEnum)
            {
                return null;
            }
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Array ar = t.GetEnumValues();
            string tmp;
            foreach (int i in ar)
            {
                tmp = t.GetEnumName(i);
                if (tmp != null)
                {
                    dic.Add(tmp, i);
                }
            }
            return dic;
        }
        #endregion
        #endregion
    }
}
