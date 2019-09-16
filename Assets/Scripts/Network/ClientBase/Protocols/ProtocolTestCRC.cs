using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    class ProtocolTestCRC : ProtocolBase
    {
        public byte[] bytes = new byte[1024];
        private int end = 0;
        public int End
        {
            get => end;
        }
        private bool hasSpace = false;
        public bool HasSpace
        {
            get => false;
        }
        private bool hasCRC = false;
        public bool HasCRC
        {
            get => hasCRC;
        }
        public ProtocolTestCRC()
        {

        }

        public override ProtocolBase Decode(byte[] readbuff, int start, int length)
        {
            ProtocolTestCRC protocol = new ProtocolTestCRC();
            protocol.bytes = new byte[length];
            Array.Copy(readbuff, start, protocol.bytes, 0, length);
            return protocol;
        }

        public override byte[] Encode()
        {
            return bytes;
        }

        public void AddInt(int num)
        {
            if (End + SF.INT_SIZE > bytes.Length)
                return;
            byte[] numBytes = BitConverter.GetBytes(num);
            Array.Copy(numBytes, 0, bytes, end, SF.INT_SIZE);
            end += SF.INT_SIZE;
        }

        public int GetInt(int start)
        {
            if (bytes == null || bytes.Length < End + SF.INT_SIZE)
                return 0;
            end += SF.INT_SIZE;
            return BitConverter.ToInt32(bytes, start);
        }

        public int GetInt(int start, ref int _end)
        {
            if (bytes == null || bytes.Length < End + SF.INT_SIZE)
                return 0;
            _end += SF.INT_SIZE;
            return BitConverter.ToInt32(bytes, start);
        }

        public void AddString(string s)
        {
            if (s.Length + end + SF.INT_SIZE > bytes.Length)
                return;
            AddInt(s.Length);
            byte[] strBytes = Encoding.UTF8.GetBytes(s);
            Array.Copy(strBytes, 0, bytes, end, s.Length);
            end += strBytes.Length;
        }

        public string GetString(int start, ref int _end)
        {
            if (end + SF.INT_SIZE > bytes.Length)
                return "";
            int strL = GetInt(start, ref _end);
            if (end + SF.INT_SIZE + strL > bytes.Length)
                return "";
            string str = Encoding.UTF8.GetString(bytes, _end, strL);
            _end += strL;
            return str;
        }

        public string GetString(int start)
        {
            return null;
        }

        public void GetCRC16Space()
        {
            if (hasSpace)
                return;
            byte[] b = new byte[end + 2];
            Array.Copy(bytes, b, end + 2);
            bytes = b;
            hasSpace = true;
        }
        public void GetCRC16()
        {
            if (!HasSpace)
                GetCRC16Space();
            CRC16 cRC16 = new CRC16(bytes, hasSpace);
            cRC16.CRC_16();
            bytes = cRC16.AppendCRC();
        }

        public void GetCRC8Space()
        {
            if (hasSpace)
                return;
            byte[] b = new byte[end + 1];
            Array.Copy(bytes, b, end + 1);
            bytes = b;
            hasSpace = true;
        }

        public void GetCRC8()
        {
            if (!HasSpace)
                GetCRC8Space();
            CRC8 crc = new CRC8(bytes, hasSpace);
            crc.CRC_8();
            bytes = crc.AppendCRC();
        }
    }
}
