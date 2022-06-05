using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSet
{
    public Color[] warmColors;
    public Color[] coldColors;

    public Color[] GetColorsByType(ColorType type)
    {
        switch (type) {
            case ColorType.warm:
                return warmColors;
            case ColorType.cold:
                return coldColors;
            case ColorType.invalid:
                throw new System.Exception("ColorType is invalid");
        }
        throw new System.Exception("Current ColorSet does not correspond to ColorType");
    }
}

public enum ColorType
{
    invalid = 0,
    warm = 1,
    cold = 2
}
