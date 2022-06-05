using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicChestLoot : ScriptableObject, IBarElement, IChestInfo
{
    [SerializeField]
    private ClassicChest.Type chestModel;
    [SerializeField]
    private Sprite barIcon;

    [SerializeField]
    private int maxItems;

    [SerializeField]
    private int minMoney;
    [SerializeField]
    private int maxMoney;

    [SerializeField]
    private AbstractGiveHpStrategy giveHPStrategy;

    [SerializeField]
    private int locationIndex;
    [SerializeField]
    private bool goldAnimation;
    [SerializeField]
    private SpecialChestAnimation specialAnimationIndex;

    [SerializeField]
    private GameObject healthPointPrefab;

    [SerializeField]
    private Loot[] loots;

    public ClassicChest.Type ChestModel { get => chestModel; set => chestModel = value; }
    public int LocationIndex { get => locationIndex; set => locationIndex = value; }
    public bool GoldAnimation { get => goldAnimation; set => goldAnimation = value; }
    public SpecialChestAnimation SpecialAnimationIndex { get => specialAnimationIndex; set => specialAnimationIndex = value; }
    public Sprite ColoredBarIcon => barIcon;
    public Color BarIconColor => Color.white;

    public Sprite WhiteBarIcon => throw new System.NotImplementedException();

    GameObject IChestInfo.ChestModel => throw new System.NotImplementedException();

    public int GenerateRandomMoney()
    {
        return Random.Range(minMoney, maxMoney + 1);
    }

    public void Init(PageCollection collection)
    {
        throw new System.NotImplementedException();
    }

    public LootInfo Unpack()
    {
        List<GameObject> result = new List<GameObject>();

        int hpCount = Mathf.Min(maxItems, giveHPStrategy.GetHealthPointsCount());

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, "HP"),
                new Parameter(FirebaseAnalytics.ParameterValue, hpCount)
            });
        MetaSceneData.Statistics.HpDelta += hpCount;
        MetaSceneData.Statistics.AddHP += hpCount;

        for (int i = 0; i < hpCount; i++)
            result.Add(healthPointPrefab);
        
        foreach(Loot loot in loots) {

            bool end = false;

            foreach(int chanse in loot.CountsAndChances) {

                if (result.Count == maxItems || end)
                    break;

                int r = Random.Range(0, 101);
                if (r <= chanse)
                    result.Add(loot.Collection.RandomItem());
                else {
                    end = true;
                    break;
                }
            }
        }

        return new LootInfo() {
            lootObjects = result
            // not work
        };
    }

    [System.Serializable]
    public class Loot
    {
        [SerializeField] [Range(0, 100)]
        private int[] countsAndChances;
        public int[] CountsAndChances { get => countsAndChances; set => countsAndChances = value; }

        [SerializeField]
        private ItemCollection collection;
        public ItemCollection Collection { get => collection; set => collection = value; }

    }

}


