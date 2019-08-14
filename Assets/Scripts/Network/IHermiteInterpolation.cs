using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHermiteInterpolation
{
    /// <summary>
    /// 两点三次埃尔米特插值
    /// </summary>
    /// <param name="start">第一个已知点</param>
    /// <param name="end">第二个已知点</param>
    /// <param name="forecast">需要预测的点</param>
    /// <returns>预测点的 Vector3</returns>
    Vector3 Hermite(Vector3 start, Vector3 end, float forecast);

    /// <summary>
    /// 三点埃尔米特插值
    /// </summary>
    /// <param name="start">第一个已知点</param>
    /// <param name="mid">第二个已知点</param>
    /// <param name="end">第三个已知点</param>
    /// <param name="forecast">需要预测的点</param>
    /// <returns>预测点的 Vector3</returns>
    Vector3 Hermite(Vector3 start, Vector3 mid, Vector3 end, float forecast);
}
