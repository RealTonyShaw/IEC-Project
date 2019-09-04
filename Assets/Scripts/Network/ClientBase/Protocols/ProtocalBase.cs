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
    public enum ProtoName
    {
        //base
        SyncUnitProp = 0,
        FeedbackUnitProp = 1,
        SyncUnitTransform = 2,
        SyncFireInfo = 3,
        SyncUpgradeInfo = 4,
        UpgradeUnitInfo = 5,
        UpdateInfo = 6,
        CreateUnit = 7,
        SyncMaxVBonus = 9,
        SyncTurningVBonus = 10,
        //database
        Login = 100,
        Logout = 101,
        Register = 102,
        OnLoginBack = 103,
        //player interaction
        RoomChat = 200,
        HallChat = 201,
        //server detection
        TimeTest = 300,
        TimeRecv = 301,
        TimeSet = 302,
        HeatBeat = 303,
        //rooms
        PlayerDataInit = 400,
        SetSelectShipType = 401,
        StartFight = 402,
        CancelReady = 403,
        PlayerLeave = 404,
        CreateRoom = 405,
        OnMaxPlayerChange = 406,
        OnCreateRoomBack = 407,
        OnMaxPlayerValueChangeBack = 408,
        GetRoomList = 409,
        OnGetRoomListBack = 410,
        UpdateRoomList = 411,
        SetMaxPlayer = 412,
        JoinRoom = 413,
        QuitRoom = 414,
        OnJoinRoomBack = 415,
        OnQuitRoomBack = 416,
        Ready = 417,
        ReadyCancel = 418,
        StartGame = 419,
        OnStartGameBack = 420,
        OnReadyBack = 421,
        OnReadyCancelBack = 422,
        Match = 423,
        GameStart = 424,
        CancelMatch = 425,
    }
}
