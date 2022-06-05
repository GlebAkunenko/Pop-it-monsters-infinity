using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New collection", menuName = "Collection", order = 51)]
public class PageCollection : ScriptableObject
{
    [SerializeField]
    private string uniqueName = "";
    [SerializeField]
    private GameObject[] common;
    [SerializeField]
    private GameObject[] rare;
    [SerializeField]
    private GameObject[] epic;
    [SerializeField]
    private GameObject[] legendary;
    [SerializeField]
    private GameObject[] defaultMonsters;

    private List<string> unlockedMonsters = new List<string>();

    public int AllMonstersCount => common.Length + rare.Length + epic.Length + legendary.Length;

    public ReadOnlyCollection<string> UnlockedMonsters => unlockedMonsters.AsReadOnly();

    public bool Loaded => unlockedMonsters.Count > 0;

    public GameObject[] DefaultMonsters => defaultMonsters;

    public InfinityLevelData OwnerLocationData { get; set; }

    private List<string> lastUnlockedMonster = new List<string>();
    public List<string> LastUnlockedMonster
    {
        get => lastUnlockedMonster;
        set
        {
            lastUnlockedMonster = value;
            DataSaver.SetKey(GetLastUnlockedMonsterKeyName(), LastUnlockedMonster);
        }
    }

    private string GetUnlockedMonstersKeyName()
    {
        if (uniqueName == "")
            throw new System.Exception("Collection name is not set");
        return uniqueName + "_collection";
    }

    private string GetLastUnlockedMonsterKeyName()
    {
        if (uniqueName == "")
            throw new System.Exception("Collection name is not set");
        return uniqueName + "_lastUnlock";
    }

    public void LoadUnlockedMonsters()
    {
        if (DataSaver.HasKey(GetUnlockedMonstersKeyName()))
            unlockedMonsters = (List<string>)DataSaver.GetKey(GetUnlockedMonstersKeyName());
        else {
            LoadDefaultUnlockedMonaters();
            SaveUnlockedMonsters();
        }
        if (DataSaver.HasKey(GetLastUnlockedMonsterKeyName()))
            LastUnlockedMonster = (List<string>)DataSaver.GetKey(GetLastUnlockedMonsterKeyName());
        else
            DataSaver.SetKey(GetLastUnlockedMonsterKeyName(), LastUnlockedMonster);
    }

    public void SaveUnlockedMonsters()
    {
        DataSaver.SetKey(GetUnlockedMonstersKeyName(), unlockedMonsters);
    }

    public void LoadDefaultUnlockedMonaters()
    {
        foreach(GameObject m in defaultMonsters) {
            if (!UnlockedMonsters.Contains(m.name))
                unlockedMonsters.Add(m.name);
        }
    }

    public void UnlockMonster(string monsterName)
    {
        unlockedMonsters.Add(monsterName);
        LastUnlockedMonster.Add(monsterName);
        SaveUnlockedMonsters();
    }

    public GameObject[] GetAllMonsters(Rarity rarity)
    {
        switch (rarity) {
            case Rarity.common:
                return common;
            case Rarity.rare:
                return rare;
            case Rarity.epic:
                return epic;
            case Rarity.legendary:
                return legendary;
        }

        throw new System.Exception();
    }

    public GameObject[] GetAllUnlockedMonsters()
    {
        List<GameObject> list = new List<GameObject>();
        List<GameObject> all = new List<GameObject>();
        all.AddRange(common);
        all.AddRange(rare);
        all.AddRange(epic);
        all.AddRange(legendary);
        foreach(GameObject a in all) {
            if (UnlockedMonsters.Contains(a.name))
                list.Add(a);
        }
        return list.ToArray();
    }

    public GameObject[] GetUnlockedMonsters(Rarity rarity)
    {
        List<GameObject> list = new List<GameObject>();
        GameObject[] all = GetAllMonsters(rarity);
        foreach (GameObject a in all) {
            if (UnlockedMonsters.Contains(a.name))
                list.Add(a);
        }
        return list.ToArray();
    }

    public GameObject[] GetAllLockedMonsters(Rarity rarity)
    {
        List<GameObject> list = new List<GameObject>();
        GameObject[] all = GetAllMonsters(rarity);
        foreach(GameObject monster in all) {
            if (UnlockedMonsters.Contains(monster.name) == false)
                list.Add(monster);
        }

        return list.ToArray();
    }

    public void RenameAllLinkedFiles()
    {
        List<GameObject> all = new List<GameObject>();
        all.AddRange(common);
        all.AddRange(rare);
        all.AddRange(epic);
        all.AddRange(legendary);
        foreach (GameObject a in all)
            a.GetComponentInChildren<Monster>().RenameLinkedFiles();
    }

    /// <summary>
    /// Only for debug
    /// </summary>
    public void ClearUnlockedMonsters()
    {
        unlockedMonsters.Clear();
    }

    //public void UpdateMonsters()
    //{
    //    List<GameObject> all = new List<GameObject>();
    //    all.AddRange(common);
    //    all.AddRange(rare);
    //    all.AddRange(epic);
    //    all.AddRange(legendary);
    //    foreach (GameObject a in all)
    //        a.GetComponentInChildren<Monster>().UpdateData();
    //}
}
