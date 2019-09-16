using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBase
{
    //协议基类，所有协议继承自这个
    public class ProtocolBase
    {

        /// <summary>
        /// 解码器，解码readbuff中从start开始的length字节
        /// </summary>
        /// <param name="readbuff"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual ProtocolBase Decode(byte[] readbuff, int start, int length)
        {
            return new ProtocolBase();
        }

        /// <summary>
        /// 编码器
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Encode()
        {
            return new byte[] { };
        }

        /// <summary>
        /// 协议名称，用于消息分发
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return "";
        }

        public virtual string GetNameX(int start, ref int end)
        {
            return "";
        }

        public virtual string GetNameX()
        {
            return "";
        }

        /// <summary>
        /// 用于调试的时候比较直观地显示协议内容
        /// </summary>
        /// <returns></returns>
        public virtual string GetDesc()
        {
            return "";
        }

        public virtual int GetNumber()
        {
            return 0;
        }

        public virtual bool IsCorrect()
        {
            return false;
        }
    }    
}
