using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointDrop : InfinityDropItem
{
    protected override void UpdatePlayerData()
    {
        PlayerStorageData.GetInstance().HealthCoins += 1;
    }
}
