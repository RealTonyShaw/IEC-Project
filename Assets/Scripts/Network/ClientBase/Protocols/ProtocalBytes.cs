using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientBase
{
    public class ProtocolBytes : ProtocolBase
    {
        //传输的字节流
        public byte[] bytes;

        public override ProtocolBase Decode(byte[] readbuff, int start, int length)
        {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.bytes = new byte[length];
            //Console.WriteLine(length.ToString());
            Array.Copy(readbuff, start, protocol.bytes, 0, length);
            return protocol;
        }


        /// <summary>
        /// 编码器
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            return bytes;
        }

        /// <summary>
        /// 协议名称
        /// </summary>
        /// <returns></returns>
        public override string GetName()
        {
            return GetString(0);
        }

        public override string GetDesc()
        {
            string str = "";
            if (bytes == null) return str;
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = (int)bytes[i];
                str += b.ToString() + " ";
            }
            return str;
        }

        public static ProtocolBytes GetProtocolHead(ProtoName protoName)
        {
            ProtocolBytes proto = new ProtocolBytes();
            proto.AddInt((int)protoName);
            return proto;
        }

        public override int GetNumber()
        {
            return GetByte(0);
        }

        #region 字符串相关
        public void AddString(string str)
        {
            short len = (short)str.Length;
            byte[] lenBytes = BitConverter.GetBytes(len);
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            if (bytes == null)
                bytes = lenBytes.Concat(strBytes).ToArray();
            else
                bytes = bytes.Concat(lenBytes).Concat(strBytes).ToArray();
            //Debug.Log(GetDesc());
            return;
        }


        public string GetString(int start, ref int end)
        {
            //若为空则返回空
            if (bytes == null) return "";
            //若起始位置后不满四位（存放字节长度的四位字节），则返回
            if (bytes.Length < start + sizeof(short)) return "";
            Int32 strLen = BitConverter.ToInt16(bytes, start);
            //若数据包长度未达到标记的值，则返回空
            if (bytes.Length < start + sizeof(short) + strLen) return "";

            string str = Encoding.UTF8.GetString(bytes, start + sizeof(short), strLen);
            end = start + sizeof(short) + strLen;
            return str;
        }


        public string GetString(int start)
        {
            int end = 0;
            return GetString(start, ref end);
        }
        #endregion

        #region Int相关
        public void AddInt(int num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }
            return;
        }

        public int GetInt(int start, ref int end)
        {
            if (bytes == null) return 0;
            if (bytes.Length < start + sizeof(Int32)) return 0;
            end = start + sizeof(Int32);
            return BitConverter.ToInt32(bytes, start);
        }

        public int GetInt(int start)
        {
            int end = 0;
            return GetInt(start, ref end);
        }
        #endregion

        #region Float相关
        public void AddFloat(float num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }
            return;
        }

        public float GetFloat(int start, ref int end)
        {
            if (bytes == null) return 0;
            if (bytes.Length < start + sizeof(float)) return 0;
            end = start + sizeof(float);
            return BitConverter.ToSingle(bytes, start);
        }

        public float GetFloat(int start)
        {
            int end = 0;
            return GetInt(start, ref end);
        }
        #endregion

        #region Long相关
        public void AddLong(long num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }
            return;
        }

        public long GetLong(int start, ref int end)
        {
            if (bytes == null) return 0;
            if (bytes.Length < start + sizeof(long)) return 0;
            end = start + sizeof(long);
            return BitConverter.ToInt64(bytes, start);
        }

        public long GetLong(int start)
        {
            int end = 0;
            return GetLong(start, ref end);
        }
        #endregion

        #region Short相关
        public void AddShort(Int16 num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }
            return;
        }

        public Int16 GetShort(int start, ref int end)
        {
            if (bytes == null) return 0;
            if (bytes.Length < start + sizeof(Int16)) return 0;
            end = start + sizeof(Int16);
            return BitConverter.ToInt16(bytes, start);
        }

        public Int16 GetShort(int start)
        {
            int end = 0;
            return GetShort(start, ref end);
        }
        #endregion

        #region
        public void AddByte(byte num)
        {
            byte[] numBytes = { num };
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }
            return;
        }

        public byte GetByte(int start, ref int end)
        {
            if (bytes == null) return 0;
            if (bytes.Length < start + sizeof(byte)) return 0;
            end = start + sizeof(byte);
            return bytes[start];
        }

        public byte GetByte(int start)
        {
            int end = 0;
            return GetByte(start, ref end);
        }
        #endregion

        #region
        public void AddChar(char num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }
            return;
        }

        public char GetChar(int start, ref int end)
        {
            if (bytes == null) return '0';
            if (bytes.Length < start + sizeof(char)) return '0';
            end = start + sizeof(char);
            return BitConverter.ToChar(bytes, start);
        }

        public char GetChar(int start)
        {
            int end = 0;
            return GetChar(start, ref end);
        }
        #endregion

        #region 通过数字头获取协议名
        public override string GetNameX(int start, ref int end)
        {
            return GetProtoName(start, ref end);
        }

        public override string GetNameX()
        {
            return GetProtoName();
        }

        public string GetProtoName(int start, ref int end)
        {
            int protoEnum = GetByte(start, ref end);
            ProtoName protoName;
            try
            {
                protoName = (ProtoName)protoEnum;
                return protoName.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return "";
        }

        public string GetProtoName()
        {
            int protoEnum = GetByte(0);
            ProtoName protoName;
            try
            {
                protoName = (ProtoName)protoEnum;
                return protoName.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return "";
        }
        #endregion

        /// <summary>
        /// Append crc code. If length > 144 use crc-16 else use crc-8.
        /// </summary>
        public override void AppendCrc()
        {
            if (bytes.Length > 144)
            {
                CRC16 crc = new CRC16(bytes, false);
                crc.CRC_16();
                bytes = crc.AppendCRC();
            }
            else
            {
                CRC8 crc = new CRC8(bytes, false);
                crc.CRC_8();
                bytes = crc.AppendCRC();
            }
        }
    }
}
