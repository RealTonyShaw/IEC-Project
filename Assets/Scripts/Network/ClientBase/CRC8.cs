using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    public class CRC8
    {
        private byte[] data;
        private bool isInit = false;
        private bool hasCRC = false;
        public byte res = 0xFF;
        public const uint poly = 0x31;
        public CRC8()
        {

        }
        public CRC8(byte[] _data)
        {
            byte[] b = { 0 };
            data = _data.Concat(b).ToArray();
            isInit = true;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_data">要校验的数据</param>
        /// <param name="op">是否是校验crc8</param>
        public CRC8(byte[] _data, CRC_op op)
        {
            if (op == CRC_op.Generate)
            {
                byte[] b = { 0 };
                data = _data.Concat(b).ToArray();
                isInit = true;
            }
            else
            {
                data = _data;
                isInit = true;
                CRC_8();
            }
        }
        /// <summary>
        /// 初始化crc8
        /// </summary>
        /// <param name="_data">数据</param>
        /// <param name="hasSpace">是否为CRC8的码留了位置</param>
        public CRC8(byte[] _data, bool hasSpace)
        {
            InitCRC(_data, hasSpace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="hasSpace">是否预留了存放CRC8的末位</param>
        public void InitCRC(byte[] _data, bool hasSpace)
        {
            if (!hasSpace)
            {
                byte[] b = { 0x0 };
                data = _data.Concat(b).ToArray();
                res = 0xFF;
                hasCRC = false;                
            }
            else
            {
                data = _data;
                res = 0xFF;
                hasCRC = false;
            }
            isInit = true;
        }
        /// <summary>
        /// 生成CRC8，并且存在res里面，然后返回
        /// </summary>
        /// <returns></returns>
        public byte CRC_8()
        {
            if (!isInit)
                return 0xFF;
            res = 0xFF;
            int bitLength = data.Length << 3, bit = 0;
            for (int i = 0; i < bitLength; i++)
            {
                if ((res & 0x80) != 0)
                {
                    res = (byte)(res ^ poly);
                }
                bit = GetBit(i) == 0 ? 0 : 0x1;
                res = (byte)((res << 1) | bit);
            }
            hasCRC = true;
            return res;
        }
        /// <summary>
        /// 判断是否是正确的码
        /// </summary>
        /// <returns></returns>
        public bool IsCorrect()
        {            
            return isInit && res == 0;
        }

        public bool IsError()
        {
            return !(isInit && res == 0);
        }
        public int GetBit(int bit)
        {
            int index = bit >> 3;
            int bias = bit & 0x7;
            switch (bias)
            {
                case 0:
                    return data[index] & 0x80;
                case 1:
                    return data[index] & 0x40;
                case 2:
                    return data[index] & 0x20;
                case 3:
                    return data[index] & 0x10;
                case 4:
                    return data[index] & 0x8;
                case 5:
                    return data[index] & 0x4;
                case 6:
                    return data[index] & 0x2;
                case 7:
                    return data[index] & 0x1;
                default:
                    return -1;
            }
        }
        /// <summary>
        /// 为字节数组添加CRC，并且返回
        /// </summary>
        /// <returns>返回的就是类内部的，注意修改</returns>
        public byte[] AppendCRC()
        {
            if (!isInit)
                return null;
            if (hasCRC)
            {                
                data[data.Length - 1] = (byte)(data[data.Length - 1] | res);
            }
            return data;
        }
        #region 打印数据
        public static void ViewByte(byte b)
        {
            for (int i = 7; i >= 0; i--)
            {
                Console.Write((b & (1 << i)) != 0 ? 1 : 0);
            }
            Console.WriteLine();
        }
        private void ViewData()
        {
            int l = data.Length * 8;
            for (int i = 0; i < l; i++)
            {
                Console.Write(GetBit(i) == 0 ? 0 : 1);
            }
            Console.WriteLine();
        }
        #endregion
    }

    public enum CRC_op
    {
        Generate = 0,
        Judge = 1
    }
}
