using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientBase;
using System;

public static class DataSync
{
    #region Server & Client
    /// <summary>
    /// Heart beat, 5 or more seconds per
    /// </summary>
    public static void SyncTimeCheck()
    {        
        AppendCRC8AndSend(SF.GetProtocolHead(ProtoName.SyncTimeCheck));
    }

    /// <summary>
    /// More frequent than time check
    /// </summary>
    public static void Ping()
    {
        AppendCRC8AndSend(SF.GetProtocolHead(ProtoName.Ping));
    }
    #endregion

    #region Sync Movement
    public static void SyncTransform(Unit unit, long instant, Vector3 position, Vector3 rotation, Vector3 velocity, float speed)
    {
        //8 bits
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncTransform);
        //32 bits
        protocol.AddInt((int)instant);
        //8 bits
        protocol.AddByte((byte)unit.attributes.ID);
        //96 bits
        AppendVector3(protocol, position);
        //96 bits
        AppendVector3(protocol, rotation);
        //96 bits
        AppendVector3(protocol, velocity);
        //32 bits
        protocol.AddFloat(speed);

        AppendCRC16AndSend(protocol);
    }


    //public static void SyncAcceleration(Unit unit, long instant, int acceleration, int angularAcceleration, Vector3 cameraForward)
    //{
    //    ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncAcceleration);
    //    protocol.AddLong(instant);
    //    protocol.AddShort((short)unit.GetInstanceID());
    //    protocol.AddInt(acceleration);
    //}
    #endregion

    #region SyncInput
    /// <summary>
    /// 同步移动控制轴的值。即水平轴和垂直轴的值。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    /// <param name="h">水平轴的值 Horizontal Axis</param>
    /// <param name="v">垂直轴的值 Vertical Axis</param>
    public static void SyncMobileControlAxes(Unit unit, long instant, int h, int v)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMobileControlAxes);
        protocol.AddInt((int)instant);
        protocol.AddByte((byte)unit.attributes.ID);
        protocol.AddByte(PackHaV(h, v));
        AppendCRC8AndSend(protocol);
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
        AppendCRC8AndSend(protocol);
    }
    /// <summary>
    /// 同步鼠标左键按下事件。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    public static void SyncMouseButton0Down(Unit unit, long instant)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMouseButton0Down);//16 bits
        protocol.AddInt((int)instant);//32 bits
        protocol.AddByte((byte)unit.attributes.ID); ;//8 bits 
        AppendCRC8AndSend(protocol);
    }

    /// <summary>
    /// 同步鼠标左键松开事件。
    /// </summary>
    /// <param name="instant">发生时刻</param>
    public static void SyncMouseButton0Up(Unit unit, long instant)
    {
        ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncMouseButton0Up);//16 bits
        protocol.AddInt((int)instant);//32 bits
        protocol.AddByte((byte)unit.attributes.ID); ;//8 bits 
        AppendCRC8AndSend(protocol);
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

        AppendCRC8AndSend(protocol);
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

        AppendCRC8AndSend(protocol);
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
        AppendCRC8AndSend(protocol);
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
        AppendCRC8AndSend(protocol);
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
        Console.WriteLine("a = " + Convert.ToString(a, 2));
        a = a | (v & 3);
        Console.WriteLine("a = " + Convert.ToString(a, 2));
        return (byte)a;
    }

    public static int[] ParseHaV(byte data)
    {
        int[] res = new int[2];
        res[1] = ParseBitResult(3 & data);
        res[0] = ParseBitResult((12 & data) >> 2);
        return res;
    }

    public static int ParseBitResult(int n)
    {
        return n == 3 ? -1 : n;
    }
    #endregion

    #region CRC
    public static void AppendCRC16AndSend(ProtocolBase protocol)
    {
        CRC16 crc = new CRC16(protocol.Encode(), false);
        crc.CRC_16();
        crc.AppendCRC();
        Client.Instance.Send(protocol);
    }

    public static void AppendCRC8AndSend(ProtocolBase protocol)
    {
        CRC8 crc = new CRC8(protocol.Encode(), false);
        crc.CRC_8();
        crc.AppendCRC();
        Client.Instance.Send(protocol);
    }
    #endregion
}
