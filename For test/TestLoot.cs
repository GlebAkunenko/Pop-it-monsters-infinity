using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestLoot : MonoBehaviour
{
    [SerializeField]
    private Image monster;
    [SerializeField]
    private TextMeshProUGUI id;
    [SerializeField]
    private TextMeshProUGUI healthCoins;
    [SerializeField]
    private TextMeshProUGUI gold;
    [SerializeField]
    private Text chance;
    [SerializeField]
    private Text[] chances;
    [Space(10)]
    [SerializeField]
    private Image background;
    [SerializeField] private Color common;
    [SerializeField] private Color rare;
    [SerializeField] private Color epic;
    [SerializeField] private Color legendary;

    private Color baseColor;

    private void Start()
    {
        baseColor = background.color;
    }

    private Color ColorWithRarity(Rarity rarity)
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
        throw new System.Exception("invalid enum");
    }

    private string MyParse(float number)
    {
        if (number == 0)
            return "0";
        if (number < 1) {
            string s = "0.";
            for (int i = 0; i < 9; i++) {
                number *= 10;
                s += ((int)number).ToString();
                number -= (int)number;
            }
            return s;
        }
        return number.ToString();
    }

    public void Set(int id, int hp, int money, bool no, Sprite monster, Rarity rarity, TestLootInfo info)
    {
        background.color = baseColor;

        healthCoins.text = hp.ToString();
        gold.text = money.ToString();
        this.id.text = id.ToString();
        chance.text = info.monsterDropChance.ToString();
        for (int i = 0; i < 4; i++) {
            chances[i].text = info.chanses[i].ToString();
            if (i == 3)
                chances[i].text = MyParse(info.chanses[i]);
        }
        if (monster != null) 
            this.monster.sprite = monster;
        else
            this.monster.enabled = false;
        if (rarity != Rarity.invalid)
            background.color = ColorWithRarity(rarity);
    }
}
