using System;
using UnityEngine;

[Serializable]
public class SerializingVector
{
    public float x;
    public float y;
    public float z;

    public SerializingVector(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
