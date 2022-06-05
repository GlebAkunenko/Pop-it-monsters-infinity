using UnityEngine;

public struct MonsterBarElement : IBarElement
{
    public MonsterBarElement(Monster monster, Color barIconColor)
    {
        ColoredBarIcon = monster.ColoredBarIcon;
        WhiteBarIcon = monster.WhiteBarIcon;
        BarIconColor = barIconColor;
        Monster = monster;
    }

    public Sprite ColoredBarIcon { private set; get; }

    public Sprite WhiteBarIcon { private set; get; }

    public Color BarIconColor { private set; get; }

    public Monster Monster { private set; get; }
}
