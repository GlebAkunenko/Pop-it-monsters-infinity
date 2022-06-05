using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InfinityLevelData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private PageCollection monstersCollection;
    //[SerializeField]
    //[Range(1, 3)]
    //private int health = 1;
    [SerializeField]
    private Sprite[] backgrounds;

    private Monster[] unlockedMonstersCache;

    public bool TimeLevel { get; set; }
    public bool IsInited { private set; get; }
    public int Id { get => id; set => id = value; }
    public PageCollection MonstersCollection => monstersCollection;

    private SavableInt offset;
    private SavableInt backgroundIndex;
    private SavableInt openedChestCount;
    private Savable<ColorType> lastMonsterColorType;

    public int Offset
    {
        get => offset;
        set => offset.Set(value);
    }

    public int BackgroundIndex
    {
        get => backgroundIndex;
        set => backgroundIndex.Set(value);
    }

    public int OpenedChests
    {
        get => openedChestCount; 
        set => openedChestCount.Set(value);
    }

    public ColorType LastMonsterColorType
    {
        get => lastMonsterColorType;
        set => lastMonsterColorType.Set(value);
    }

    public void Init()
    {
        offset = new SavableInt(0, string.Format("location_{0}_offset", id));
        backgroundIndex = new SavableInt(0, string.Format("location_{0}_back", id));
        openedChestCount = new SavableInt(0, string.Format("location_{0}_chests", id));
        lastMonsterColorType = new Savable<ColorType>(ColorType.warm, GetLastMonsterColorTypeKeyName());

        if (monstersCollection != null)
            monstersCollection.OwnerLocationData = this;
        else
            Debug.LogWarning("on some location collection is not set!");

        IsInited = true;
    }

    public Monster[] GetAllUnlockedMonsters()
    {
        if (unlockedMonstersCache == null || unlockedMonstersCache.Length != monstersCollection.UnlockedMonsters.Count)
            InitUnlockedMonsters();

        return unlockedMonstersCache;
    }

    public Sprite PopBackground()
    {
        BackgroundIndex++;
        return backgrounds[BackgroundIndex % backgrounds.Length];
    }

    private void InitUnlockedMonsters()
    {
        GameObject[] unlockedMonstersGameObjects = monstersCollection.GetAllUnlockedMonsters();
        int n = unlockedMonstersGameObjects.Length;
        unlockedMonstersCache = new Monster[n];
        for (int i = 0; i < n; i++)
            unlockedMonstersCache[i] = unlockedMonstersGameObjects[i].GetComponentInChildren<Monster>();
    }

    private string GetLastMonsterColorTypeKeyName()
    {
        return string.Format("location_{0}_colorType", id);
    }

}
