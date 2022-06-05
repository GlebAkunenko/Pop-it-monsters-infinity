using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestButtonImage : MonoBehaviour
{
    [SerializeField]
    private string location;
    [SerializeField]
    private ClassicChestLoot loot;

    public string Location { get => location; set => location = value; }
    public ClassicChestLoot Loot { get => loot; set => loot = value; }
}
