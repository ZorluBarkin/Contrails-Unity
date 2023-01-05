using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherScript : MonoBehaviour
{
    // might change this to a animation curve for extra accuracy in the future, this is fast and inexpensive
    public readonly float[] airDensity = { 1.225f, 1.112f, 1.007f, 0.9093f, 0.8194f, 0.7364f, 0.6601f, 0.5900f, 0.5258f, 0.4671f, 0.4135f, 0.1948f, 0.08891f};
                                            //0         1000    2000    3000    4000    5000        6000    7000    8000    9000    10000       15000   20000
}
