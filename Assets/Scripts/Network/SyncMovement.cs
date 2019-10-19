using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovement : ISyncMovement
{
    const float Ms2Sec = 0.001f;
    Unit unit;
    bool recv_Ac = false;
    bool recv_T = false;
    int recvAc_cnt = 0;
    int recvT_cnt = 0;
    ObjectBuffer<AccelerationData> AcData = new ObjectBuffer<AccelerationData>(2);
    ObjectBuffer<TransformData> tData = new ObjectBuffer<TransformData>(2);
    // 埃尔米特插值
    IHermiteInterpolation interpolate = new HermiteInterpolation();

    Quaternion tRot;
    Vector3 tCamFwd;
    Vector3 tPos;

    // Unit 的组件
    Mover mover;
    Rigidbody rb;


    public void Init(Unit unit)
    {
        this.unit = unit;
        mover = unit.GetComponent<Mover>();
        rb = unit.GetComponent<Rigidbody>();
    }

    public void SyncAcceleration(long instant, int v, int h, Vector3 camFwd)
    {
        AccelerationData data = new AccelerationData(instant, v, h, camFwd);
        AcData.Buffer(data);
        if (!recv_Ac)
        {
            recvAc_cnt++;
        }
        // 直接同步角加速度和加速度
        mover.H = h;
        mover.V = v;
        // 推测 camFwd
        Quaternion rot1 = Quaternion.LookRotation(AcData.Get(1).fwd);
        Quaternion rot2 = Quaternion.LookRotation(camFwd);
        Quaternion drot = Quaternion.Inverse(rot1) * rot2;
        tCamFwd = drot * camFwd;
        if (!recv_Ac && recvAc_cnt >= 2)
        {
            recv_Ac = true;
        }
    }

    public void SyncTransform(long instant, Vector3 position, Quaternion rotation, float speed)
    {
        TransformData data = new TransformData(instant, position, rotation, rotation * Vector3.forward * speed);
        tData.Buffer(data);
        if (!recv_T)
        {
            recvT_cnt++;
        }
        tPos = position + data.v * ((Gamef.SystemTimeInMillisecond - instant + GameDB.SYNC_TRANSFORM_INTERVAL >> 1) * 1e-3f);
        // 推测姿态
        Quaternion rot1 = tData.Get(1).rot;
        Quaternion rot2 = rotation;
        Quaternion drot = Quaternion.Inverse(rot1) * rot2;
        tRot = rotation * drot;
        if (!recv_T && recvT_cnt >= 2)
        {
            recv_T = true;
        }
    }

    public void Update(float dt)
    {
        if (recv_T)
        {
            //// 对 unit 的三维坐标进行插值
            //TransformData d1, d2;
            //d1 = tData.Get(1);
            //d2 = tData.Get(0);
            //Vector3 tpos = HermiteInterpolate(d1.instant * Ms2Sec, d1.pos, d1.v, d2.instant * Ms2Sec, d2.pos, d2.v, Gamef.SystemTime);
            // 修改 unit 的位置
            unit.transform.position = Vector3.Lerp(unit.transform.position, tPos, 2f * dt);
            // rotation
            unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, tRot, 2f * dt);
        }

        if (recv_Ac)
        {
            // cameraForward
            mover.CameraForward = Vector3.Slerp(mover.CameraForward, tCamFwd, 2f * dt);
        }
    }

    Vector3 HermiteInterpolate(float t1, Vector3 p1, Vector3 v1, float t2, Vector3 p2, Vector3 v2, float t)
    {
        Vector3 res;
        res.x = interpolate.Hermite(t1, p1.x, v1.x, t2, p2.x, v2.x, t);
        res.y = interpolate.Hermite(t1, p1.y, v1.y, t2, p2.y, v2.y, t);
        res.z = interpolate.Hermite(t1, p1.z, v1.z, t2, p2.z, v2.z, t);
        return res;
    }

    private class ObjectBuffer<T>
    {
        private readonly List<T> list = new List<T>();
        private readonly int bufferedTimes;
        /// <summary>
        /// 创建一个对象缓存。
        /// </summary>
        /// <param name="bufferedTimes">最多缓存的对象个数</param>
        public ObjectBuffer(int bufferedTimes)
        {
            this.bufferedTimes = Mathf.Clamp(bufferedTimes, 1, 99);
            for (int i = 0; i < bufferedTimes; i++)
            {
                list.Add(default);
            }
        }
        /// <summary>
        /// 获得指定序号的对象。越早被缓存的对象，其序号越大。
        /// </summary>
        /// <param name="index">对象的序号</param>
        /// <returns>对应的对象</returns>
        public T Get(int index)
        {
            return list[Mathf.Clamp(index, 0, bufferedTimes - 1)];
        }

        /// <summary>
        /// 将对象缓存。缓存已满时，最早的缓存对象会被丢弃。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>被丢弃的对象</returns>
        public T Buffer(T obj)
        {
            list.Insert(0, obj);
            T res = list[bufferedTimes];
            list.RemoveAt(bufferedTimes);
            return res;
        }
    }

    private struct AccelerationData
    {
        public long instant;
        public Vector3 fwd;
        public int v;
        public int h;
        public AccelerationData(long instant, int v, int h, Vector3 fwd)
        {
            this.instant = instant;
            this.v = v;
            this.h = h;
            this.fwd = fwd;
        }
    }

    private struct TransformData
    {
        public long instant;
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 v;
        public TransformData(long instant, Vector3 pos, Quaternion rot, Vector3 v)
        {
            this.instant = instant;
            this.pos = pos;
            this.rot = rot;
            this.v = v;
        }
    }
}
