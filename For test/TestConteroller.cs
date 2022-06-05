using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConteroller : MonoBehaviour
{
    [SerializeField]
    private PageCollection collection;
    [SerializeField]
    private TestChestLoot chestLoot;
    [SerializeField]
    private TestLoot[] tests;

    private int hp;
    private int gold;

    [SerializeField]
    private StepFunction function;

    private AnalyticsResult GetUserInfo()
    {
        ResetInfo();

        int n = 180;
        bool[] b = new bool[4];
        var result = new AnalyticsResult();

        for (int i = 1; i < n + 1; i++) {
            var info = chestLoot.Unpack();

            if (info.dropedMonster != null) {
                if (!collection.UnlockedMonsters.Contains(info.dropedMonster.name))
                    collection.UnlockMonster(info.dropedMonster.name);

                if (info.monsterRarity == Rarity.epic) {
                    result.DropEpic(i);
                    Debug.Log("!");
                }

                if (info.monsterRarity == Rarity.legendary)
                    result.Leg = i;
            }

            for (int r = 0; r < 4; r++) {
                if (collection.GetAllLockedMonsters((Rarity)r).Length == 0 && !b[r]) {
                    result.AllAt[r] = i;
                    b[r] = true;
                }
            }

        }

        return result;
    }

    private void ResetInfo()
    {
        collection = Instantiate(collection);
        collection.ClearUnlockedMonsters();
        collection.LoadDefaultUnlockedMonaters();
        //new InfinityLevelData() { Id = -1, monstersCollection = collection }.Init();
        chestLoot.Init(collection);

        TestChestLoot.openedChest = 0;

        //string d = "";
        //foreach (string s in collection.UnlockedMonsters)
        //    d += s + "\n";
        //Debug.Log(d);

        hp = 0;
        gold = 0;
    }

    private void SetTest(TestLoot test, int id)
    {
        var info = chestLoot.Unpack();
        hp += info.hp;
        gold += info.money;
        if (info.lootObjects.Count > 0) {
            test.Set(
                id: id,
            hp: hp,
            money: gold,
            no: info.no,
            monster: info.lootObjects[0].GetComponentInChildren<Monster>().BookIcon,
            rarity: info.monsterRarity,
            info
            );
            collection.UnlockMonster(info.lootObjects[0].name);
        }
        else
            test.Set(
                id: id,
            hp: hp,
            money: gold,
            no: info.no,
            monster: null,
            rarity: info.monsterRarity,
            info
            );
    }

    public void EnterText(string text)
    {
        ResetInfo();

        int n = int.Parse(text);
        for (int i = 0; i < n; i++)
            SetTest(tests[i], i + 1);
    }

    public void TestWhile()
    {
        int n = 100;
        List<int> firstEpic = new List<int>();
        List<int> secondEpic = new List<int>();
        List<int> leg = new List<int>();
        for (int i = 0; i < n; i++) {
            AnalyticsResult userInfo = GetUserInfo();
            firstEpic.Add(userInfo.FirstEpic);
            secondEpic.Add(userInfo.SecondEpic);
            leg.Add(userInfo.Leg);
        }
        string s = "";
        int min = int.MaxValue;
        int sum = 0;
        foreach (int fe in firstEpic) {
            s += fe.ToString() + " ";
            min = Mathf.Min(min, fe);
            sum += fe;
        }
        Debug.Log(s);
        Debug.Log(min.ToString() + " " + ((float)sum/n).ToString());

        s = "";
        min = int.MaxValue;
        sum = 0;
        foreach (int fe in secondEpic) {
            s += fe.ToString() + " ";
            min = Mathf.Min(min, fe);
            sum += fe;
        }
        Debug.Log(s);
        Debug.Log(min);
        Debug.Log(min.ToString() + " " + ((float)sum / n).ToString());


        s = "";
        min = int.MaxValue;
        sum = 0;
        foreach (int fe in leg) {
            s += fe.ToString() + " ";
            min = Mathf.Min(min, fe);
            sum += fe;
        }
        Debug.Log(s);
        Debug.Log(min);
        Debug.Log(min.ToString() + " " + ((float)sum / n).ToString());
    }



    public class AnalyticsResult
    {
        private int[] allAt = new int[4];
        private int firstEpic;
        private int secondEpic;
        private int leg;

        public int FirstEpic { get => firstEpic; set => firstEpic = value; }
        public int SecondEpic { get => secondEpic; set => secondEpic = value; }
        public int Leg { get => leg; set => leg = value; }
        public int[] AllAt { get => allAt; set => allAt = value; }

        public string AllRarityAnalyticsSum(Rarity rarity)
        {
            return string.Format("All {0} on {1}", rarity.ToString(), AllAt[(int)rarity].ToString());
        }

        public void DropEpic(int chest)
        {
            if (firstEpic == 0)
                firstEpic = chest;
            else
                secondEpic = chest;
        }

        public void ShowResult()
        {
            string s = "";
            for (int r = 0; r < 4; r++)
                s += AllRarityAnalyticsSum((Rarity)r) + "\n";
            s += "\n";

            s += string.Format("Epics on {0} and {1}\n", firstEpic, secondEpic);
            s += "Leg on " + Leg.ToString();
            Debug.Log(s);
        }
        
    }

}
