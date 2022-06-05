using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPageModel : MonoBehaviour
{
    [SerializeField]
    private PageMarkup[] markups;

    private PageMarkup currentMarkup;

    public void AnimateUnlockMonster()
    {
        currentMarkup.TryAnimateUnlockMonter();
    }

    public void SetPageCollection(PageCollection collection)
    {
        if (collection != null)
        if (!collection.Loaded)
            collection.LoadUnlockedMonsters();

        foreach (PageMarkup markup in markups) {
            if (markup.Collection == collection) {
                currentMarkup = markup;
                markup.gameObject.SetActive(true);
                markup.InitCells();
            }
            else
                markup.gameObject.SetActive(false);
        }
    }

}
