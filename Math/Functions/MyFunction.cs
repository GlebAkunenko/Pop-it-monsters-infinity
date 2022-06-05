using UnityEngine;

public abstract class MyFunction : MonoBehaviour
{
    public float Func(float x)
    {
        return Func(Mathf.RoundToInt(x));
    }

    public abstract float Func(int x);

    public abstract string GeoGebraUrl { get; }
}
