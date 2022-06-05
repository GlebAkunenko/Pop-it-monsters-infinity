using System.Collections.Generic;
using UnityEngine;

public interface IChestInfo
{
    public GameObject ChestModel { get; }

    public void Init(PageCollection collection);

    public LootInfo Unpack();
}

public class LootInfo
{
    public List<GameObject> lootObjects;
    public int money;
    public GameObject dropedMonster;
    public AudioClip music;
    public PageCollection collection;
}