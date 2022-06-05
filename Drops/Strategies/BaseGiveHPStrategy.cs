using UnityEngine;

public class BaseGiveHPStrategy : AbstractGiveHpStrategy
{
    [SerializeField]
    private int minHealthPoints;
    [SerializeField]
    private int maxHealthPoints;

    public override int GetHealthPointsCount()
    {
        return Random.Range(minHealthPoints, maxHealthPoints + 1);
    }
}
