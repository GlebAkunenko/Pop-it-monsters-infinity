using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingInfChestLoot : MonoBehaviour, IChestInfo, IBarElement
{
    [SerializeField]
    private Sprite barIcon;
    [SerializeField]
    private int healthPointsCount;
    [SerializeField]
    private int minMoney;
    [SerializeField]
    private int maxMoney;
    [SerializeField]
    private AudioClip monsterDropMusic;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject healthCoinPrefab;
    [SerializeField]
    private GameObject goldsPrefab;
    [SerializeField]
    private GameObject monsterDropPrefab;

    private GameObject dropMonster;
    private PageCollection collection;
    private bool inited;

    public GameObject ChestModel => gameObject;

    public Sprite ColoredBarIcon => barIcon;

    public Color BarIconColor => Color.white;

    public Sprite WhiteBarIcon => null;

    public void Init(PageCollection collection)
    {
        this.collection = collection;

        GameObject[] defaultMonsters = collection.DefaultMonsters;
        for (int i = 0; i < defaultMonsters.Length; i++) {
            if (!collection.UnlockedMonsters.Contains(defaultMonsters[i].name)) {
                dropMonster = defaultMonsters[i];
                break;
            }
        }
        inited = true;
    }

    public LootInfo Unpack()
    {
        if (!inited)
            throw new System.Exception("Script is not inited!");

        int goldsCount = Random.Range(minMoney, maxMoney + 1);

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < healthPointsCount; i++)
            objects.Add(healthCoinPrefab);

        InfGoldsDrop golds = goldsPrefab.GetComponent<InfGoldsDrop>();
        objects.Add(goldsPrefab);

        objects.Add(monsterDropPrefab);

        return new LootInfo() {
            lootObjects = objects,
            money = goldsCount,
            dropedMonster = dropMonster,
            music = monsterDropMusic,
            collection = collection
        };
    }
}
