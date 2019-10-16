using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载以启用跟踪 gameobject 功能
/// </summary>
public class Follow : MonoBehaviour
{
    public float followingRate;
    private GameObject obj;
    // 如果将 offsetX 默认置 0，则实例与玩家同位置
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    private void Awake()
    {
        obj = GameCtrl.PlayerUnit.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(obj.transform.position.x - offsetX, obj.transform.position.y - offsetY, obj.transform.position.z - offsetZ) , followingRate);
    }
}
