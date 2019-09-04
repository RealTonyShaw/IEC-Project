using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    public class CRC16
    {
        private byte[] data;
        private bool isInit = false;
        private bool hasCRC = false;
        public ushort res = 0xFFFF;
        public const ushort poly = 0x1021;

        public CRC16()
        {

        }
        /// <summary>
        /// This constructor is for ascertain if the protocol is proper.
        /// </summary>
        /// <param name="_data"></param>
        public CRC16(byte[] _data)
        {
            data = _data;
            isInit = true;
            CRC_16();
        }
        public CRC16(byte[] _data, bool hasSpace)
        {
            InitCRC(_data, hasSpace);
            isInit = true;
        }
        public void InitCRC(byte[] _data)
        {
            byte[] b = { 0x0, 0x0 };
            data = _data.Concat(b).ToArray();
            res = 0xFFFF;
            hasCRC = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="hasSpace">是否预留了存放CRC16的末位</param>
        public void InitCRC(byte[] _data, bool hasSpace)
        {
            if (!hasSpace)
            {
                byte[] b = { 0x0, 0x0 };
                data = _data.Concat(b).ToArray();
                res = 0xFFFF;
                isInit = true;
                hasCRC = false;
            }
            else
            {
                data = _data;
                res = 0xFFFF;
                isInit = true;
                hasCRC = false;
            }
        }

        public ushort CRC_16()
        {
            if (!isInit)
                return 0xFFFF;
            res = 0x0;
            int bitLength = data.Length << 3, bit = 0;
            for (int i = 0; i < bitLength; i++)
            {
                if ((res & 0x8000) != 0)
                    res = (ushort)(res ^ poly);
                bit = GetBit(i) == 0 ? 0 : 0x1;
                //Console.Write(bit);
                res = (ushort)((res << 1) | bit);
            }
            hasCRC = true;
            //Console.WriteLine();
            return res;
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
        /// 为数组添加CRC。返回的是类内部的bytes数组
        /// </summary>
        /// <returns>返回的是类内部的字节数组，可能是原来的数组，注意修改</returns>
        public byte[] AppendCRC()
        {
            if (!isInit)
                return null;
            if (hasCRC)
            {
                data[data.Length - 2] = (byte)(data[data.Length - 2] | (res >> 8));
                data[data.Length - 1] = (byte)(data[data.Length - 1] |  res);
            }
            return data;
        }

        public bool IsCorrect()
        {
            return res == 0 && isInit;
        }
    }
}
