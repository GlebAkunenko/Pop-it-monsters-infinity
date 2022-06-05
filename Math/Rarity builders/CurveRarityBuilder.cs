using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarveRaritySettings", menuName = "Rarity builders/Curve rarities", order = 51)]
public class CurveRarityBuilder : RarityBuilder
{
    [SerializeField] private AnimationCurve commonChanceFunc;
    [SerializeField] private AnimationCurve rareChanceFunc;
    [SerializeField] private AnimationCurve epicChanceFunc;
    [SerializeField] private AnimationCurve legendaryChanceFunc;

    private AnimationCurve GetRarityCurve(Rarity rarity)
    {
        switch (rarity) {
            case Rarity.common:
                return commonChanceFunc;
            case Rarity.rare:
                return rareChanceFunc;
            case Rarity.epic:
                return epicChanceFunc;
            case Rarity.legendary:
                return legendaryChanceFunc;
        }
        throw new System.Exception();
    }

    public override float RarityProbability(Rarity rarity)
    {
        if (rarity == Rarity.common && collection.UnlockedMonsters.Count < 3)
            return 1;

        int unlocked = collection.GetUnlockedMonsters(rarity).Length;
        int all = collection.GetAllMonsters(rarity).Length;
        if (unlocked > all)
            throw new System.Exception();
        if (unlocked == all)
            return 0;

        return Mathf.Clamp(GetRarityCurve(rarity).Evaluate(unlocked), 0f, 1f);
    }
}
