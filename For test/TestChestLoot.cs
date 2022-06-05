using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChestLoot : InfChestLoot
{
    public static int openedChest;

    private bool TryToDropMonster(out GameObject dropedMonster, out Rarity rarity)
    {
        dropedMonster = null;
        rarity = Rarity.invalid;
        if (!CheckDropMonsterChance())
            return false;

        if (rarityBuilder != null) {
            if (!rarityBuilder.TryGetDropRarity(out rarity))
                return false;
        }
        else {
            if (!GetRarityFromFunctions(openedChest, out rarity))
                return false;
        }

        GameObject[] monsters = collection.GetAllLockedMonsters(rarity);
        if (monsters.Length == 0)
            return false;

        dropedMonster = monsters[Random.Range(0, monsters.Length)];
        return true;
    }

    public TestLootInfo Unpack()
    {
        if (!inited)
            throw new System.Exception("Script is not inited!");

        int healthPointsCount = Random.Range(minHealthPoints, maxHealthPoints + 1);
        int goldsCount = Random.Range(minGold, maxGold + 1);

        GameObject monster;
        Rarity rarity;
        List<GameObject> m = new List<GameObject>();
        if (TryToDropMonster(out monster, out rarity))
            m.Add(monster);

        float chance = baseMonsterDropChance;
        if (collection.UnlockedMonsters.Count < 3)
            chance = 1;
        if (collection.UnlockedMonsters.Count == collection.AllMonstersCount)
            chance = 0;
        float[] c = new float[4];
        if (rarityBuilder != null) {
            for (int i = 0; i < 4; i++)
                c[i] = rarityBuilder.MonsterChance((Rarity)i, chance);
        }
        else {
            c[0] = GetFunctionRarityChance(Rarity.common, openedChest);
            c[1] = GetFunctionRarityChance(Rarity.rare, openedChest) * (1 - c[0]);
            c[2] = GetFunctionRarityChance(Rarity.epic, openedChest) * (1 - c[1]) * (1 - c[0]);
            c[3] = GetFunctionRarityChance(Rarity.legendary, openedChest) * (1 - c[2]) * (1 - c[1]) * (1 - c[0]);
        }

        float sumChance = 0;
        foreach (float f in c)
            sumChance += f;

        openedChest++;

        return new TestLootInfo() {
            lootObjects = m,
            hp = healthPointsCount,
            money = goldsCount,
            dropedMonster = monster,
            collection = collection,
            monsterRarity = rarity,
            monsterDropChance = sumChance,
            chanses = c
        };
    }
}


public class TestLootInfo : LootInfo
{
    public int hp;
    public bool no;
    public Rarity monsterRarity;
    public float monsterDropChance;
    public float[] chanses;
}