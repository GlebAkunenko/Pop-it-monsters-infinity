using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomRaritySettings", menuName = "Rarity builders/Random rarities", order = 51)]
public class RandomRarityBuilder : RarityBuilder
{
    [Header("Wights")]
    [SerializeField] [Range(0, 100)]
    private int common;
    [SerializeField] [Range(0, 100)] 
    private int rare;
    [SerializeField] [Range(0, 100)] 
    private int epic;
    [SerializeField] [Range(0, 100)] 
    private int legendary;

    public override bool TryGetDropRarity(out Rarity result)
    {
        int[] rarities = new int[] { common, rare, epic, legendary };
        
        int maximum = 0;
        foreach (int r in rarities)
            maximum += r;

        int random = Random.Range(0, maximum + 1);
        int offset = 0;

        for(int r = 0; r < 4; r++) {
            if (rarities[r] + offset >= random) {
                result = (Rarity)r;
                return true;
            }
            offset += rarities[r];
        }

        throw new System.Exception();
    }

    public override float RarityProbability(Rarity rarity)
    {
        int[] rarities = new int[] { common, rare, epic, legendary };

        int maximum = 0;
        foreach (int r in rarities)
            maximum += r;

        switch (rarity) {
            case Rarity.common:
                return (float)common / maximum;
            case Rarity.rare:
                return (float)rare / maximum;
            case Rarity.epic:
                return (float)epic / maximum;
            case Rarity.legendary:
                return (float)legendary / maximum;
        }

        throw new System.Exception();
    }
}

public enum Rarity
{
    invalid = -1,
    common = 0, 
    rare = 1, 
    epic = 2,
    legendary = 3
}
