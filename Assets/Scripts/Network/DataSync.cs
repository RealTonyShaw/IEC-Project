using ClientBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

public static class DataSync
{
    #region Server & Client

    #region Login
    public static void Login(string username, string password)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.Login);
        //MD5 md5p = MD5.Create(password);
        protocol.AddString(username);
        protocol.AddString(GetMd5(password));
        ClientLauncher.Instance.Local_Player = username;
        Client.Instance.Send(protocol);
    }

    public static void Logout()
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.Logout);
        Client.Instance.Send(protocol);
    }

    public static void Ready()
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.Logout);
        Client.Instance.Send(protocol);
    }

    public static void ReadyCancel()
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.Logout);
        Client.Instance.Send(protocol);
    }

    /// <summary>
    /// To apply for register.
    /// </summary>
    /// <param name="username">At least 8 bits</param>
    /// <param name="password">At least 8 bits</param>
    public static void Register(string username, string password)
    {
        if (username == null || password == null)
        {
            return;
        }
        if (username.Length < 1 || password.Length < 1)
        {
            return;
        }
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.Register);
        protocol.AddString(username);
        protocol.AddString(GetMd5(password));
        Client.Instance.Send(protocol);
    }
    #endregion

    #region Base
    /// <summary>
    /// Heart beat, 5 or more seconds per
    /// </summary>
    public static void SyncTimeCheck()
    {
        Client.Instance.Send(SF.GetProtocolHead(ProtoName.SyncTimeCheck));
    }

    /// <summary>
    /// More frequent than time check
    /// </summary>
    public static void Ping()
    {
        Client.Instance.Send(SF.GetProtocolHead(ProtoName.Ping));
    }

    public static void Chatting(string msg)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.Chatting);
        protocol.AddString(msg);
        Client.Instance.Send(protocol);
    }

    public static void Match()
    {
        if (Client.Instance.isConnect && Client.Instance.pl_info.isLogin)
        {
            Client.Instance.Send(SF.GetProtocolHead(ProtoName.Match));
        }
    }

    public static void CanControll()
    {
        if (Client.Instance.isConnect && Client.Instance.pl_info.isLogin)
        {
            Client.Instance.Send(SF.GetProtocolHead(ProtoName.CanControll));
        }
    }

    public static void CancelMatch()
    {
        if (Client.Instance.isConnect && Client.Instance.pl_info.isLogin)
        {
            Client.Instance.Send(SF.GetProtocolHead(ProtoName.CancelMatch));
        }
    }
    #endregion

    #region Net Object
    /// <summary>
    /// Send a message to server, which includes the message of unit and applies for permission.
    /// </summary>
    /// <param name="unit">The unit type enum</param>
    /// <param name="isLocal">Is this object local</param>
    /// <param name="position">The position of the unit that would be created at</param>
    /// <param name="rotation">The rotation of the unit</param>
    public static void CreateObject(int playerID, UnitName unit, Vector3 position, Quaternion rotation)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.CreateObject);
        protocol.AddByte((byte)playerID);// playerID
        protocol.AddByte((byte)unit);//UnitName
        AppendVector3(protocol, position);//Position
        AppendQuaternion(protocol, rotation);//Rotation
        Client.Instance.Send(protocol);
    }

    /// <summary>
    /// Destroy a object.
    /// </summary>
    /// <param name="id">The id of the unit that being destroyed</param>
    public static void DestroyObj(byte id)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.DestroyObj);
        protocol.AddByte(id);
        Client.Instance.Send(protocol);
    }
    #endregion
    #endregion

    #region Sync Movement
    public static void SyncTransform(Unit unit, long instant, Vector3 position, Quaternion rotation, float speed)
    {
        Debug.Log(string.Format("unit {0} send position {1}", unit.attributes.ID, position));
        //8 bits
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncTransform);
        //32 bits
        protocol.AddInt((int)instant);
        //8 bits
        protocol.AddByte((byte)unit.attributes.ID);
        //96 bits
        AppendVector3(protocol, position);
        //128 bits
        AppendQuaternion(protocol, rotation);
        //32 bits
        protocol.AddFloat(speed);

        Client.Instance.Send(protocol);
    }
    #endregion

    #region SyncInput
    /// <summary>
    /// 同步移动控制轴的值。即水平轴和垂直轴的值。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    /// <param name="h">水平轴的值 Horizontal Axis</param>
    /// <param name="v">垂直轴的值 Vertical Axis</param>
    public static void SyncMobileControlAxes(Unit unit, long instant, int h, int v, Vector3 cameraFwd)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMobileControlAxes);
        protocol.AddInt((int)instant);//instant
        protocol.AddByte((byte)unit.attributes.ID);//unit id
        protocol.AddByte(PackHaV(h, v));//h and v
        AppendVector3(protocol, cameraFwd);// camera forward
        Debug.Log(string.Format("unit {0} send ac cam = {1} h = {2} v = {3}", unit.attributes.ID, cameraFwd, h, v));
        Client.Instance.Send(protocol);
    }


    /// <summary>
    /// 同步切换到的技能栏的序号。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    /// <param name="skillIndex">技能栏序号</param>
    public static void SyncSwitchSkill(Unit unit, long instant, int skillIndex)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncSwitchSkill);//16 bits
        protocol.AddInt((int)instant);//32 bits
        protocol.AddByte((byte)unit.attributes.ID);//8 bits 
        protocol.AddByte((byte)skillIndex);//32 bits
        Client.Instance.Send(protocol);
    }
    /// <summary>
    /// 同步鼠标左键按下事件。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    public static void SyncMouseButton0Down(Unit unit, long instant)
    {
        Debug.Log(string.Format("unit {0} send btn down casting at {1}", unit.attributes.ID, instant));
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMouseButton0Down);//16 bits
        protocol.AddInt((int)instant);//32 bits
        protocol.AddByte((byte)unit.attributes.ID); ;//8 bits 
        Client.Instance.Send(protocol);
    }

    /// <summary>
    /// 同步鼠标左键松开事件。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    public static void SyncMouseButton0Up(Unit unit, long instant)
    {
        Debug.Log(string.Format("unit {0} send btn up casting at {1}", unit.attributes.ID, instant));
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMouseButton0Up);//16 bits
        protocol.AddInt((int)instant);//32 bits
        protocol.AddByte((byte)unit.attributes.ID); ;//8 bits 
        Client.Instance.Send(protocol);
    }
    #endregion

    #region Player Casting State
    /// <summary>
    /// 同步技能开始施法。
    /// </summary>
    /// <param name="instant">开始施法的时刻</param>
    /// <param name="skillIndex">技能序号，指玩家的技能槽位的序号，取值1,2,3,4</param>
    public static void SyncStart(Unit unit, long instant, int skillIndex)
    {
        //8 bits
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncStart);
        //32 bits
        protocol.AddInt((int)instant);
        //8 bits
        protocol.AddByte((byte)unit.attributes.ID);
        protocol.AddByte((byte)skillIndex);

        Client.Instance.Send(protocol);
    }

    /// <summary>
    /// 同步技能停止施法。
    /// </summary>
    /// <param name="instant">停止施法的时刻</param>
    /// <param name="skillIndex">技能序号，指玩家的技能槽位的序号，取值1,2,3,4</param>
    public static void SyncStop(Unit unit, long instant, int skillIndex)
    {
        //8 bits
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncStart);
        //32 bits
        protocol.AddInt((int)instant);
        //8 bits
        protocol.AddByte((byte)unit.attributes.ID);
        //8 bits
        protocol.AddByte((byte)skillIndex);

        Client.Instance.Send(protocol);
    }

    public static void SyncAimTarget(long instant, int sourceId, int targetId)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncAimTarget);
        protocol.AddInt((int)instant);
        protocol.AddByte((byte)sourceId);
        protocol.AddByte((byte)targetId);

        Client.Instance.Send(protocol);
    }
    #endregion

    #region Unit state

    /// <summary>
    /// 同步单位的生命值。
    /// </summary>
    /// <param name="instant">时刻，即单位处于该生命值的时刻</param>
    /// <param name="HP">单位的生命值</param>
    public static void SyncHP(Unit unit, long instant, float HP)
    {
        //8 bits
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncHP);
        //32 bits
        protocol.AddInt((int)instant);
        //8 bits
        protocol.AddByte((byte)unit.attributes.ID);
        //32 bits
        protocol.AddFloat(HP);
        Client.Instance.Send(protocol);
    }

    /// <summary>
    /// 同步单位的魔法值。
    /// </summary>
    /// <param name="instant">时刻，即单位处于该魔法值的时刻</param>
    /// <param name="MP">单位的魔法值</param>
    public static void SyncMP(Unit unit, long instant, float MP)
    {
        //8 bits
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMP);
        //32 bits
        protocol.AddInt((int)instant);
        //8 bits
        protocol.AddByte((byte)unit.attributes.ID);
        //32 bits
        protocol.AddFloat(MP);
        Client.Instance.Send(protocol);
    }
    #endregion

    #region Data Handler
    public static void AppendVector3(ProtocolBytes protocol, Vector3 vector)
    {
        if (protocol == null)
        {
            return;
        }
        protocol.AddFloat(vector.x);
        protocol.AddFloat(vector.y);
        protocol.AddFloat(vector.z);
    }

    public static Vector3 ParseVector3(ProtocolBase protocol, ref int start)
    {
        if (protocol == null)
        {
            return Vector3.zero;
        }

        try
        {
            ProtocolBytes proto = (ProtocolBytes)protocol;
            float x, y, z;
            x = proto.GetFloat(start, ref start);
            y = proto.GetFloat(start, ref start);
            z = proto.GetFloat(start, ref start);
            return new Vector3(x, y, z);

            //return new Vector3(protocol.)
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return Vector3.zero;
    }


    public static void AppendQuaternion(ProtocolBytes protocol, Quaternion quaternion)
    {
        if (protocol == null)
        {
            return;
        }
        protocol.AddFloat(quaternion.x);
        protocol.AddFloat(quaternion.y);
        protocol.AddFloat(quaternion.z);
        protocol.AddFloat(quaternion.w);
    }

    public static Quaternion ParseQuaternion(ProtocolBase protocol, ref int start)
    {
        if (protocol == null)
        {
            return Quaternion.identity;
        }

        try
        {
            ProtocolBytes proto = (ProtocolBytes)protocol;
            float x, y, z, w;
            x = proto.GetFloat(start, ref start);
            y = proto.GetFloat(start, ref start);
            z = proto.GetFloat(start, ref start);
            w = proto.GetFloat(start, ref start);
            return new Quaternion(x, y, z, w);
            //return new Vector3(protocol.)
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return Quaternion.identity;
    }

    public static byte PackHaV(int h, int v)
    {
        int a = h;
        a = a << 2;
        //Debug.Log("a = " + Convert.ToString(a, 2));
        a = a | (v & 3);
        //Debug.Log("a = " + Convert.ToString(a, 2));
        //Debug.Log(string.Format("Pack : h = {0}, v = {1}", h, v));
        return (byte)a;
    }

    public static int[] ParseHaV(byte data)
    {
        int[] res = new int[2];
        res[1] = ParseBitResult(3 & data);
        res[0] = ParseBitResult((12 & data) >> 2);
        //Debug.Log(string.Format("Recv : h = {0}, v = {1}", res[0], res[1]));
        return res;
    }

    public static int ParseBitResult(int n)
    {
        return n == 3 ? -1 : n;
    }

    public static string GetMd5(string password)
    {
        MD5 md5p = new MD5CryptoServiceProvider();
        byte[] bs = md5p.ComputeHash(Encoding.Default.GetBytes(password));
        string pw = "";
        for (int i = 0; i < bs.Length; i++)
        {
            pw += string.Format("{0:x}", bs[i]);
        }
        return pw;
    }
    #endregion

    //#region CRC
    //public static void AppendCrcAndSend(ProtocolBase protocol)
    //{
    //    if (protocol.Encode().Length > 128)
    //    {
    //        AppendCRC16AndSend(protocol);
    //    }
    //    else
    //    {
    //        AppendCRC8AndSend(protocol);
    //    }
    //}

    //public static void AppendCRC16AndSend(ProtocolBase protocol)
    //{
    //    CRC16 crc = new CRC16(protocol.Encode(), false);
    //    crc.CRC_16();
    //    crc.AppendCRC();
    //    Client.Instance.Send(protocol);
    //}

    //public static void AppendCRC8AndSend(ProtocolBase protocol)
    //{
    //    CRC8 crc = new CRC8(protocol.Encode(), false);
    //    crc.CRC_8();
    //    crc.AppendCRC();
    //    Client.Instance.Send(protocol);
    //}
    //#endregion
}
