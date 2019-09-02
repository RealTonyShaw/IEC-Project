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
    float Hermite(float x0, float y0, float derivative0, float x1, float y1, float derivative1, float forecastX);
}
