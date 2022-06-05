using System.Collections.Generic;
using UnityEngine;

public class ChallangeMonsters : ChallangeStack
{
    [SerializeField]
    private ChallangeMonster[] monsters;

    private List<MonsterBarElement> barElements;

    public override List<MonsterBarElement> BarElements
    { 
        get 
        {
            if (barElements == null) {
                barElements = new List<MonsterBarElement>();
                foreach (ChallangeMonster monster in monsters)
                    barElements.Add(new MonsterBarElement(monster.prefab.GetComponentInChildren<Monster>(), monster.color));
            }
            return barElements;
        }
    }

    [System.Serializable]
    public struct ChallangeMonster
    {
        public GameObject prefab;
        public Color color;
    }
}

