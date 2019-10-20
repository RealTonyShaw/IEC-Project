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
            int playerNum = proto.GetByte(start, ref start);
            for (int i = 0; i < playerNum; i++)
            {
                ClientLauncher.Instance.playerNames[i] = proto.GetString(start, ref start);
            }
            //Loading
            GameCtrl.Instance.StartLoadingGameScene();
        }

        public static void CanControll(ProtocolBase protocol)
        {
            //...
            GameCtrl.Instance.StartCreatePlayer(Client.Instance.pl_info.id_game);
            Gamef.DelayedExecution(GameCtrl.Instance.loadingPanel.StopLoading, 0.2f);
            Gamef.DelayedExecution(delegate{ Crosshair.SetState(true); }, 0.7f);
        }

        #endregion

        #region Net Object
        //UnitName unit, Vector3 position, Quaternion rotation
        public static void CreateObject(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            int playerID = proto.GetByte(start, ref start);
            UnitName unitName = (UnitName)proto.GetByte(start, ref start);
            Vector3 pos = ParseVector3(proto, ref start);
            Quaternion rot = ParseQuaternion(proto, ref start);
            int unitId = proto.GetByte(start, ref start);

            bool isLocal = proto.GetByte(start, ref start) == 1;
            UnitData unitData = Gamef.LoadUnitData(unitName);
            GameObject prefab = isLocal ? unitData.LocalPrefab : unitData.NetPrefab;
            GameObject gameObj = Gamef.Instantiate(prefab, pos, rot);
            //set id
            Unit unit = gameObj.GetComponent<Unit>();
            unit.InitAttributes(unitId);
            if (unitName == UnitName.Player)
            {
                if (isLocal)
                {
                    CameraGroupController.Instance.ResetTransform(pos, rot);
                    GameCtrl.PlayerUnit = unit;
                }
                else
                {
                    (unit as Player).SetPlayerName(ClientLauncher.GetPlayerName(playerID));
                }
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
            Quaternion rot = ParseQuaternion(proto, ref start);
            if (unit == null)
            {
                Debug.Log(string.Format("NOT EXIST unit {0} recv sync position {1}", unit.attributes.ID, position));
                return;
            }
            float speed = proto.GetFloat(start, ref start);
            unit.SyncMovement.SyncTransform(instant, position, rot, speed);
        }
        #endregion

        #region Input
        public static void SyncMobileControlAxes(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);//parse instant
            int id = proto.GetByte(start, ref start);//parse id
            int[] hv = ParseHaV(proto.GetByte(start, ref start));//parse h and v
            Vector3 fwd = ParseVector3(proto, ref start);// parse camera forward

            Unit unit = Gamef.GetUnit(id);
            if (unit == null)
            {
                Debug.Log(string.Format("NOT EXIST unit {0} recv sync ac", unit.attributes.ID));
                return;
            }
            unit.SyncPlayerInput.SyncMobileControlAxes(instant, hv[0], hv[1], fwd);
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
            unit?.SyncPlayerInput.SyncSwitchSkill(instant, skillIndex);
        }

        public static void SyncMouseButton0Down(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            Debug.Log(string.Format("unit {0} recv btn down casting at {1}, in fact {2}", unit.attributes.ID, instant, Gamef.SystemTimeInMillisecond));
            unit?.SyncPlayerInput.SyncMouseButton0Down(instant);
        }

        public static void SyncMouseButton0Up(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int id = proto.GetByte(start, ref start);
            Unit unit = Gamef.GetUnit(id);
            Debug.Log(string.Format("unit {0} recv btn up casting at {1}, in fact {2}", unit.attributes.ID, instant, Gamef.SystemTimeInMillisecond));
            unit?.SyncPlayerInput.SyncMouseButton0Up(instant);
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

        public static void SyncAimTarget(ProtocolBase protocol)
        {
            int start = 0;
            ProtocolBytes proto = (ProtocolBytes)protocol;
            proto.GetNameX(start, ref start);
            long instant = proto.GetInt(start, ref start);
            int sourceId = proto.GetByte(start, ref start);
            int targetId = proto.GetByte(start, ref start);
            if (targetId == 255)
            {
                targetId = -1;
            }
            Unit unit = Gamef.GetUnit(sourceId);
            Debug.Log(string.Format("Send sync target ID {0} -> ID {1}", sourceId, targetId));
            unit?.SyncPlayerCastingState.SyncTarget(instant, Gamef.GetUnit(targetId));
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
