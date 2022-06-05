using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public static class FirebaseLogEvent
{
    public static void DropMonster(Rarity rarity)
    {
        FirebaseAnalytics.LogEvent("drop_monster", "rarity", (int)rarity);
    }

    public static void OpenLocationChest(int location)
    {
        FirebaseAnalytics.LogEvent("open_location_chest", "location_id", location);
    }
}

