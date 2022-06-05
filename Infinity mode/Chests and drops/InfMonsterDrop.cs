using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfMonsterDrop : InfinityDropItem
{
    [SerializeField]
    private SpriteRenderer monsterSpriteRenderer;

    private GameObject monster;
    private PageCollection collection;
    private Rarity rarity;

    [SerializeField]
    private GameObject[] rarityEffects = new GameObject[4];

    public override void Init(LootInfo lootInfo)
    {
        anim = GetComponentInChildren<Animator>();
        monster = lootInfo.dropedMonster;
        collection = lootInfo.collection;

        Monster component = monster.GetComponentInChildren<Monster>();
        monsterSpriteRenderer.sprite = component.BookIcon;
        rarity = component.Rarity;
        for (int i = 0; i < 4; i++)
            rarityEffects[i].SetActive(i == (int)component.Rarity);
    }

    protected override void OnTouch()
    {
        base.OnTouch();
        GetComponent<AudioSource>().Play();
    }

    protected override void UpdatePlayerData()
    {
        collection.UnlockMonster(monster.name);
        FirebaseLogEvent.DropMonster(rarity);
    }
}
