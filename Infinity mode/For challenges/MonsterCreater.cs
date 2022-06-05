using UnityEngine;

public class MonsterCreater
{
    protected Vector3 spawnPosition;

    public MonsterCreater(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

    public Monster CreateMonsterInstance(Monster prefab)
    {
        GameObject instance = Object.Instantiate(prefab.transform.parent.gameObject, spawnPosition, Quaternion.identity);
        Monster monsterIntance = instance.GetComponentInChildren<Monster>();
        return monsterIntance;
    }
}

