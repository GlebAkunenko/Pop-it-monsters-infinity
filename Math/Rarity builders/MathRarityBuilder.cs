using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MathRaritySettings", menuName = "Rarity builders/Math rarities", order = 51)]
public class MathRarityBuilder : RarityBuilder
{
    [SerializeField] private FunctionSettings common;
    [SerializeField] private FunctionSettings rare;
    [SerializeField] private FunctionSettings epic;
    [SerializeField] private FunctionSettings legendary;

    private FunctionSettings GetRaritySettings(Rarity rarity)
    {
        switch (rarity) {
            case Rarity.common:
                return common;
            case Rarity.rare:
                return rare;
            case Rarity.epic:
                return epic;
            case Rarity.legendary:
                return legendary;
        }
        throw new System.Exception();
    }
    
    private bool CheckChance(float chance)
    {
        return Random.value <= chance;
    }

    private float Func(float x, float N, float q, float m, float b)
    {
        float c = (q * N + q * b - b * m) / N;
        float a = b * (m - c);
        return a / (x + b) + c;
    }

    public override float RarityProbability(Rarity rarity)
    {
        int all = collection.GetAllMonsters(rarity).Length;
        int unlocked = collection.GetUnlockedMonsters(rarity).Length;
        FunctionSettings settings = GetRaritySettings(rarity);
        return Func(
            x: unlocked,
            N: all,
            q: settings.maximalChance,
            m: settings.maximalChance,
            b: settings.coefficient
            );
    }

    [System.Serializable]
    public struct FunctionSettings
    {
        [Tooltip("b")]
        public float coefficient;
        [Tooltip("m")]
        public float maximalChance;
        [Tooltip("q")]
        public float minimalChance;
    }
}