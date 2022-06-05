using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMarkup : MonoBehaviour
{
    [SerializeField]
    private PageCollection collection;
    [SerializeField]
    private MonsterCell[] cells;

    public PageCollection Collection => collection;

    public void InitCells(int orderInLayer = 0)
    {
        foreach (MonsterCell cell in cells)
            cell.Init(collection.UnlockedMonsters, orderInLayer);
    }

    public void TryAnimateUnlockMonter()
    {
        if (collection.LastUnlockedMonster.Count > 0) {
            foreach (MonsterCell cell in cells)
                cell.TryAnimateUnlock(collection.LastUnlockedMonster);
            collection.LastUnlockedMonster.Clear();
        }
    }
}

public class MonsterMarkup
{
    [SerializeField]
    private GameObject[] rarityFrames;
    [SerializeField]
    private GameObject[] rarityTexts;

    public void Init(Monster monster)
    {
        Rarity rarity = monster.Rarity;
        if (rarityFrames.Length != rarityTexts.Length)
            throw new System.Exception();
        
        for(int i = 0; i < rarityFrames.Length; i++) {
            rarityFrames[i].SetActive(i == (int)rarity);
            rarityTexts[i].SetActive(i == (int)rarity);
        }
    }
}
