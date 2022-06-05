using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfChestLoot : MonoBehaviour, IChestInfo, IBarElement
{
    [Header("Properties")]
    [SerializeField] protected int minHealthPoints;
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int minGold;
    [SerializeField] protected int maxGold;
    [Range(0f, 1f)]
    [SerializeField] protected float baseMonsterDropChance;
    [Tooltip("Only for version before 2.0.2")]
    [SerializeField]
    protected RarityBuilder rarityBuilder;
    [Tooltip("since 2.0.2")]
    [SerializeField]
    protected MyFunction[] rarityFunctions;
    [SerializeField]
    private AudioClip[] rarityMusic;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject healthCoinPrefab;
    [SerializeField]
    private GameObject goldsPrefab;
    [SerializeField]
    private GameObject monsterDropPrefab;
    [SerializeField]
    private Sprite barIcon;

    protected PageCollection collection;
    protected bool inited;

    public void Init(PageCollection collection)
    {
        this.collection = collection;
        if (rarityBuilder != null)
            rarityBuilder.Init(collection);

        inited = true;
    }

    protected bool CheckDropMonsterChance()
    {
        if (collection.AllMonstersCount == collection.UnlockedMonsters.Count)
            return false;

        if (collection.UnlockedMonsters.Count < 3)
            return true;
        return MyMath.CheckChance(baseMonsterDropChance);
    }

    protected float GetFunctionRarityChance(Rarity rarity, int openedChestCount)
    {
        if (collection.GetAllLockedMonsters(rarity).Length == 0)
            return 0;

        return Mathf.Max(0, rarityFunctions[(int)rarity].Func(openedChestCount)
                - collection.GetUnlockedMonsters(rarity).Length);
    }

    protected bool GetRarityFromFunctions(int openedChests, out Rarity rarity)
    {
        if (rarityFunctions.Length == 0)
            throw new System.Exception("Rarity functions not setted");

        for(int i = 0; i < rarityFunctions.Length; i++) {
            Rarity checkedRarity = (Rarity)i;
            if (MyMath.CheckChance(GetFunctionRarityChance(checkedRarity, openedChests))) {
                rarity = checkedRarity;
                return true;
            }
        }
        rarity = Rarity.invalid;
        return false;
    }

    /// <summary>
    /// Check monster chance and in case of success retrun true and give monster
    /// </summary>
    /// <param name="dropedMonster">In case of success dropped monster</param>
    /// <returns>True if monster will be dropped and false if chance is losed</returns>
    private bool TryToDropMonster(out GameObject dropedMonster)
    {
        dropedMonster = null;
        if (!CheckDropMonsterChance())
            return false;

        Rarity dropRarity;

        if (rarityBuilder != null) {
            if (!rarityBuilder.TryGetDropRarity(out dropRarity))
                return false;
        }
        else {
            if (!GetRarityFromFunctions(collection.OwnerLocationData.OpenedChests, out dropRarity))
                return false;
        }

        GameObject[] monsters = collection.GetAllLockedMonsters(dropRarity);
        if (monsters.Length == 0)
            return false;

        dropedMonster = monsters[Random.Range(0, monsters.Length)];
        return true;
    }

    public GameObject ChestModel => gameObject;

    public Sprite ColoredBarIcon => barIcon;

    public Color BarIconColor => Color.white;

    public Sprite WhiteBarIcon => null;

    public LootInfo Unpack()
    {
        if (!inited)
            throw new System.Exception("Script is not inited!");

        int healthPointsCount = Random.Range(minHealthPoints, maxHealthPoints + 1);
        int goldsCount = Random.Range(minGold, maxGold + 1);

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < healthPointsCount; i++)
            objects.Add(healthCoinPrefab);

        InfGoldsDrop golds = goldsPrefab.GetComponent<InfGoldsDrop>();
        objects.Add(goldsPrefab);

        GameObject monster;
        AudioClip music = null;
        if (TryToDropMonster(out monster)) {
            objects.Add(monsterDropPrefab);
            music = rarityMusic[(int)monster.GetComponentInChildren<Monster>().Rarity];
        }

        return new LootInfo() {
            lootObjects = objects,
            money = goldsCount,
            dropedMonster = monster,
            music = music,
            collection = collection
        };
    }

}

