using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteInterpolation : IHermiteInterpolation
{
    public float Hermite(float x0, float y0, float derivative0, float x1, float y1, float derivative1, float forecastX)
    {
        float part1 = y0 * (1 + 2 * (forecastX - x0) / (x1 - x0)) * ((forecastX - x1) / (x0 - x1)) * ((forecastX - x1) / (x0 - x1));
        float part2 = y1 * (1 + 2 * (forecastX - x1) / (x0 - x1)) * ((forecastX - x0) / (x1 - x0)) * ((forecastX - x0) / (x1 - x0));
        float part3 = derivative0 * (forecastX - x0) * ((forecastX - x1) / (x0 - x1)) * ((forecastX - x1) / (x0 - x1));
        float part4 = derivative1 * (forecastX - x1) * ((forecastX - x0) / (x1 - x0)) * ((forecastX - x0) / (x1 - x0));
        return part1 + part2 + part3 + part4;
    }
}