using UnityEngine;

public class BookCondition : Condition
{
    [SerializeField]
    private Rarity[] raritiesThatMustBeUnlocked;
    [SerializeField]
    private GameObject[] monstersThatMustBeUnlocked;

    public override bool Check(LocationChallengesData challengesData)
    {
        PageCollection collection = challengesData.Collection;

        foreach(Rarity rarity in raritiesThatMustBeUnlocked)
            if (collection.GetAllLockedMonsters(rarity).Length > 0)
                return false;

        foreach (GameObject monster in monstersThatMustBeUnlocked)
            if (!collection.UnlockedMonsters.Contains(monster.name))
                return false;

        return true;
    }
}