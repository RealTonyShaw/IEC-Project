using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientBase;
using System;

public static class DataSync
{
    public static void SyncTransform(Unit unit, long instant, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        ProtocolBytes protocol = new ProtocolBytes();
        //4
        protocol.AddInt((int)ProtoName.SyncTransform);
        //8
        protocol.AddLong(instant);
        //
        protocol.AddShort((short)unit.GetInstanceID());
        AppendVector3(protocol, position);
        AppendQuaternion(protocol, rotation);
        AppendVector3(protocol, velocity);
        AppendVector3(protocol, angularVelocity);

        AppendCRC16AndSend(protocol);
    }

    //public static void SyncAcceleration(Unit unit, long instant, int acceleration, int angularAcceleration, Vector3 cameraForward)
    //{
    //    ProtocolBytes protocol = SF.GetProtocolHead(ProtoName.SyncAcceleration);
    //    protocol.AddLong(instant);
    //    protocol.AddShort((short)unit.GetInstanceID());
    //    protocol.AddInt(acceleration);
    //}


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
    #endregion

    public static void AppendCRC16AndSend(ProtocolBase protocol)
    {
        CRC16 crc = new CRC16(protocol.Encode(), false);
        crc.CRC_16();
        crc.AppendCRC();
        Client.Instance.Send(protocol);
    }
}
