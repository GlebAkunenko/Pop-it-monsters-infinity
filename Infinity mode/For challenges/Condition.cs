using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : MonoBehaviour
{
    public abstract bool Check(LocationChallengesData challengesData);

    public static bool CheckSeveralConditions(Condition[] conditions, LocationChallengesData challengesData)
    {
        foreach (Condition condition in conditions)
            if (!condition.Check(challengesData))
                return false;
        return true;
    }
}
