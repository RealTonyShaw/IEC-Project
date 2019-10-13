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
        #region Base
        public static void SyncTimeCheck(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long delta = proto.GetLong(start, ref start);
            ClientLauncher.Instance.TimeCheck(delta);
        }
        public static void Ping(ProtocolBase protocol)
        {
            ClientLauncher.Instance.PingBack();
        }

        public static void Chatting(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            string str;
            Debug.Log("Receive msg from server");
            Debug.Log(str = proto.GetString(start, ref start));
            ClientLauncher.Instance.message = str;
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
                //int id = proto.GetByte(start, ref start);
                Debug.Log("Login success");
                Client.Instance.pl_info.isLogin = true;
                //Login
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                DataSync.Match();
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            }
            else
            {
                Debug.Log("Login failed");
                //Login failed
            }
        }

        //public void Logout(ProtocolBase protocol){}

        public static void Register(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            int ispass = proto.GetByte(start, ref start);
            if (ispass == 1)
            {
                //int id = proto.GetByte(start, ref start);
                Debug.Log("Reg success");
                //Login

            }
            else
            {
                Debug.Log("Reg failed");
                //Login failed
            }
        }

        public static void StartGame(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            Client.Instance.pl_info.id_game = proto.GetByte(start, ref start);

            Debug.Log("Start game!!! Loading scene!");
            //Loading
            GameCtrl.Instance.StartLoadingGameScene();
        }

        public static void CanControll(ProtocolBase protocol)
        {
            //...
            GameCtrl.Instance.StartCreatePlayer(Client.Instance.pl_info.id_game);
            Debug.Log("You can controll the player now!!!");
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
            unit.InitAttributes(unitId);
            if (unitName == UnitName.Player && isLocal)
            {
                GameCtrl.PlayerUnit = unit;
            }
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

        //public static void SyncCameraForward(ProtocolBase protocol)
        //{
        //    int start = 0;
        //    ProtocolBytes proto = (ProtocolBytes)protocol;
        //    proto.GetNameX(start, ref start);
        //    long instant = proto.GetInt(start, ref start);
        //    int id = proto.GetByte(start, ref start);
        //    Unit unit = Gamef.GetUnit(id);
        //    Vector3 fwd = ParseVector3(proto, ref start);
        //    unit.SyncMovement.SyncCameraForward(instant, fwd);
        //}
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
            Vector3 fwd = ParseVector3(proto, ref start);
            Unit unit = Gamef.GetUnit(id);
            unit.SyncPlayerInput.SyncMobileControlAxes(instant, hv[0], hv[1]);
            unit.SyncPlayerInput.SyncCameraFoward(instant, fwd);
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
