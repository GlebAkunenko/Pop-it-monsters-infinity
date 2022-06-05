using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfGoldsDrop : InfinityDropItem
{
    [SerializeField]
    private new ParticleSystem particleSystem;

    [SerializeField]
    private TextMeshPro text;

    private int amount;

    public override void Init(LootInfo loot)
    {
        base.Init(loot);
        amount = loot.money;
        text.text = amount.ToString();
    }

    protected override void OnTouch()
    {
        state++;
        particleSystem.Play();
        StartCoroutine(AddMoney());
    }

    protected override void UpdatePlayerData()
    {
        PlayerStorageData.GetInstance().Money += amount;
    }

    private IEnumerator AddMoney()
    {
        yield return new WaitForSeconds(0.9f);
        OnEndMoving();
    }

}
