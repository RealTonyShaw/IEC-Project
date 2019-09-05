using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static DataSync;
namespace ClientBase
{
    public class MsgHandler
    {
        public void SyncTransform(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            Vector3 position = ParseVector3(proto, ref start);
            Quaternion rotation = ParseQuaternion(proto, ref start);
            Vector3 velocity = ParseVector3(proto, ref start);
            Vector3 angularVelocity = ParseVector3(proto, ref start);
            unit.SyncMovement.SyncTransform(instant, position, rotation, velocity, angularVelocity);
        }

        public void SyncMobileControlAxes(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);

            int[] hv = ParseHaV(proto.GetByte(start, ref start));
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerInput.SyncMobileControlAxes(instant, hv[0], hv[1]);
        }

        public void SyncSwitchSkill(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            int skillIndex = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerInput.SyncSwitchSkill(instant, skillIndex);
        }

        public void SyncMouseButton0Down(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerInput.SyncMouseButton0Down(instant);
        }

        public void SyncMouseButton0Up(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerInput.SyncMouseButton0Up(instant);
        }
    }
}
