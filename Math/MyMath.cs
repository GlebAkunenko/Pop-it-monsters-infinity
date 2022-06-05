using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath
{
    /// <summary>Return true with a chance</summary>
    /// <param name="chance">Number between 0 and 1</param>
    public static bool CheckChance(float chance)
    {
        if (chance == 0)
            return false;
        return Random.value <= chance;
    }

    public static float RarityFunction(float x, float N, float q, float c)
    {
        float b = N * (q - c) / (1 - q);
        float a = b * (1f - c);
        return a / (x + b) + c;
    }

    public static float ChestFunction(float x, float N, float L, float q, float c)
    {
        float b = (N - L) * (q - c) / (1 - q);
        float a = b * (1f - c);
        return a / (x + b) + c;
    }
}
