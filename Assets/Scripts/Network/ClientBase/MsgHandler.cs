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
        #region Server&Client
        public void SyncTimeCheck(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long delta = proto.GetLong(start, ref start);
            ClientLauncher.Instant.TimeCheck(delta);
        }
        public void Ping()
        {
            ClientLauncher.Instant.PingBack();
        }

        #endregion

        #region Movement
        public void SyncTransform(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            Vector3 position = ParseVector3(proto, ref start);
            Vector3 rotation = ParseVector3(proto, ref start);
            Vector3 velocity = ParseVector3(proto, ref start);
            float speed = proto.GetFloat(start, ref start);
            unit.SyncMovement.SyncTransform(instant, position, rotation, velocity, speed);
        }
        #endregion

        #region Input
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
        #endregion

        #region Player Casting State
        public void SyncStart(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            int skillIndex = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerCastingState.SyncStart(instant, skillIndex);
        }

        public void SyncStop(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            int skillIndex = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerCastingState.SyncStop(instant, skillIndex);
        }

        #endregion

        #region Unit State
        public static void SyncHP(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            float val = proto.GetFloat(start, ref start);
            unit.SyncUnitState.SyncHP(instant, val);
        }

        public static void SyncMP(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            float val = proto.GetFloat(start, ref start);
            unit.SyncUnitState.SyncMP(instant, val);
        }
        #endregion
    }
}
