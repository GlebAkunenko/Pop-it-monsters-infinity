using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCell : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer monsterIcon;
    [SerializeField]
    private new Animation animation;
    [SerializeField]
    private GameObject question;

    private string monsterName;

    private bool Include(string s, ICollection<string> vs)
    {
        foreach(string v in vs) {
            if (s == v)
                return true;
        }
        return false;
    }

    public void Init(ICollection<string> unlockedMonstersName, int orderInLayer)
    {
        monsterName = monsterIcon.sprite.name;
        if (Include(monsterName, unlockedMonstersName))
            monsterIcon.color = Color.white;
        else {
            if (question == null)
                monsterIcon.color = Color.black;
            else {
                monsterIcon.enabled = false;
                question.SetActive(true);
            }

        }

        monsterIcon.sortingOrder = orderInLayer;
    }

    public void TryAnimateUnlock(List<string> unlockedMonsterNames)
    {
        if (unlockedMonsterNames.Contains(monsterName))
            animation.Play();
    }
}
