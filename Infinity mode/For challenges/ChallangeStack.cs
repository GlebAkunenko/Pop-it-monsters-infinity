using UnityEngine;
using System.Collections.Generic;

public abstract class ChallangeStack : MonoBehaviour
{
    public abstract List<MonsterBarElement> BarElements { get; }

    public int Count => BarElements.Count;

}

