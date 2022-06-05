using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterColorBuilder
{
    private int seed;
    private ColorType colorType;

    public MonsterColorBuilder(ColorType firstMonsterColorType, int seed)
    {
        this.colorType = firstMonsterColorType;
        this.seed = seed;
    }

    private ColorType GetNextColorType(ColorType last)
    {
        if (last == ColorType.invalid)
            throw new System.Exception("Invalid color type");

        if (last == ColorType.cold)
            return ColorType.warm;
        if (last == ColorType.warm)
            return ColorType.cold;

        throw new System.Exception("ColorType is wrong");
    }

    public Color GetMonsterColor(Monster monster)
    {
        Color ret = monster.GetAutoColor(seed, colorType);
        seed++;
        colorType = GetNextColorType(colorType);
        return ret;
    }

    public MonsterColorBuilder Copy()
    {
        return new MonsterColorBuilder(colorType, seed);
    }
}
