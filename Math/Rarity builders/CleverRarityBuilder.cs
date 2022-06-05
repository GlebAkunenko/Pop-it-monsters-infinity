using UnityEngine;

public class CleverRarityBuilder : RarityBuilder
{
    [SerializeField] private MyFunction commonFunc;
    [SerializeField] private MyFunction rareFunc;
    [SerializeField] private MyFunction epicFunc;
    [SerializeField] private MyFunction legendaryFunc;

    private MyFunction GetFunctionWithRarity(Rarity rarity)
    {
        switch (rarity) {
            case Rarity.common:
                return commonFunc;
            case Rarity.rare:
                return rareFunc;
            case Rarity.epic:
                return epicFunc;
            case Rarity.legendary:
                return legendaryFunc;
            default:
                throw new System.Exception("Invalid rarity");
        }
    }

    public override float RarityProbability(Rarity rarity)
    {
        MyFunction function = GetFunctionWithRarity(rarity);
        throw new System.Exception();
    }

}
