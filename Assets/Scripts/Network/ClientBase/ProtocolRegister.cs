using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    public class ProtocolRegister
    {
        private static ProtocolRegister protocolRegister;
        private Dictionary<string, int> proDic = new Dictionary<string, int>();
        public static ProtocolRegister Instance()
        {
            return protocolRegister != null ? protocolRegister : (protocolRegister = new ProtocolRegister());
        }

        private ProtocolRegister()
        {
            init();
        }

        private void init()
        {
            proDic.Add("Login", 1);
            proDic.Add("TimeTick", 2);
            proDic.Add("HeartBeat", 3);
            proDic.Add("SyncLife", 4);
        }

        /// <summary>
        /// To export a protocol messages to binary file.
        /// </summary>
        /// <param name="path">the path that store</param>
        /// <param name="name">the name of file</param>
        /// <returns></returns>
        public bool ExportProtocolMsg(string path, string name)
        {
            try
            {
                using (FileStream fs = new FileStream(path + name, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, proDic);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

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
    }
}
