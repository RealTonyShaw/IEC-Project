﻿using System;
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
        #region Base
        public static void SyncTimeCheck(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long delta = proto.GetLong(start, ref start);
            ClientLauncher.Instant.TimeCheck(delta);
        }
        public static void Ping(ProtocolBase protocol)
        {
            ClientLauncher.Instant.PingBack();
        }

        public static void Chatting(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            string str;
            Debug.Log("Receive msg from server");
            Debug.Log(str = proto.GetString(start, ref start));
            ClientLauncher.Instant.message = str;
        }
        #endregion

        #region Login
        public static void Login(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            int ispass = proto.GetByte(start, ref start);
            if (ispass == 1)
            {
                int id = proto.GetByte(start, ref start);
                //Login

            }
            else
            {
                //Login failed
                
            }
        }

        //public void Logout(ProtocolBase protocol){}

        public static void Register(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);

        }
        #endregion

        #region Net Object
        //UnitName unit, Vector3 position, Quaternion rotation
        public static void CreateObject(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            UnitName unitName = (UnitName)proto.GetByte(start, ref start);
            Vector3 pos = ParseVector3(proto, ref start);
            Quaternion rot = ParseQuaternion(proto, ref start);
            int unitId = proto.GetByte(start, ref start);
            bool isLocal = proto.GetByte(start, ref start) == 1;
            UnitData unitData = Gamef.LoadUnitData(unitName);
            GameObject prefab = isLocal ? unitData.NetPrefab : unitData.NetPrefab;
            GameObject gameObj = Gamef.Instantiate(prefab, pos, rot);
            //set id
            Unit unit = gameObj.GetComponent<Unit>();
            
        }

        public static void DestroyObj(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            //Destroy the object.
            unit.Death();
        }
        #endregion


        #endregion

        #region Movement
        public static void SyncTransform(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            Vector3 position = ParseVector3(proto, ref start);
            Vector3 forward = ParseVector3(proto, ref start);
            Vector3 up = ParseVector3(proto, ref start);
            float speed = proto.GetFloat(start, ref start);
            unit.SyncMovement.SyncTransform(instant, position, forward, up, speed);
        }
        #endregion

        #region Input
        public static void SyncMobileControlAxes(ProtocolBase protocol)
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

        public static void SyncSwitchSkill(ProtocolBase protocol)
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

        public static void SyncMouseButton0Down(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerInput.SyncMouseButton0Down(instant);
        }

        public static void SyncMouseButton0Up(ProtocolBase protocol)
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
        public static void SyncStart(ProtocolBase protocol)
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

        public static void SyncStop(ProtocolBase protocol)
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
