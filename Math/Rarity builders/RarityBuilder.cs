using UnityEngine;

public abstract class RarityBuilder : ScriptableObject
{
    protected PageCollection collection;

    public void Init(PageCollection workCollection)
    {
        collection = workCollection;
    }

    public float MonsterChance(Rarity rarity, float chestChance)
    {
        if (collection.UnlockedMonsters.Count < 3 && rarity == Rarity.common)
            return chestChance;

        float common = RarityProbability(Rarity.common);
        if (rarity == Rarity.common)
            return common * chestChance;

        float rare = (1 - common) * RarityProbability(Rarity.rare);
        if (rarity == Rarity.rare)
            return rare * chestChance;

        float epic = (1 - common) * (1 - rare) * RarityProbability(Rarity.epic);
        if (rarity == Rarity.epic)
            return epic * chestChance;

        float leg = (1 - common) * (1 - rare) * (1 - epic) * RarityProbability(Rarity.legendary);
        if (rarity == Rarity.legendary)
            return leg * chestChance;

        throw new System.Exception();
    }

    public virtual bool TryGetDropRarity(out Rarity result)
    {
        result = Rarity.invalid;
        for (int i = 0; i < 4; i++) {
            Rarity check = (Rarity)i;
            if (MyMath.CheckChance(RarityProbability(check))) {
                result = check;
                return true;
            }
        }
        return false;
    }

    public abstract float RarityProbability(Rarity rarity);
}
