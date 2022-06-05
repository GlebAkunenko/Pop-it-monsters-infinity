using UnityEngine;

public class GiveHPByCurrentAmountStrategy : AbstractGiveHpStrategy
{
    [SerializeField]
    private HealthCase[] cases;
    [SerializeField]
    private int defaultValue;

    public override int GetHealthPointsCount()
    {
        for(int i = 0; i < cases.Length; i++) {
            if (MetaSceneData.Player.HealthPoints <= cases[i].IfNotLargerThen) {
                int result = 0;
                foreach(int chance in cases[i].nextPointChance) {
                    if (Random.Range(0, 100+1) <= chance)
                        result++;
                    else return result;
                }
                return result;
            }
        }
        return defaultValue;
    }

    [SerializeField]
    private int testPlayerHP;
    private void Show10TestsOfDrops()
    {
        MetaSceneData.Player.HealthPoints = testPlayerHP;
        for (int i = 0; i < 10; i++)
            Debug.Log(GetHealthPointsCount());
    }

    [System.Serializable]
    public struct HealthCase
    {
        public int IfNotLargerThen;

        [Range(0, 100)]
        public int[] nextPointChance;
    }

}
