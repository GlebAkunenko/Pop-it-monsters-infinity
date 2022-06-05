using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropMoney : DropItem 
{
    [SerializeField]
    private new ParticleSystem particleSystem;

    [SerializeField]
    private TextMeshPro text;

    private ClassicChest chest;

    private int count;

    public void Init(int count, ClassicChest sender)
    {
        type = Type.money;
        base.Start();
        this.count = count;
        chest = sender;
        text.text = count.ToString();
    }

    private new void Update()
    {
        if (Input.touchCount > 0 && state == 1) {
            state++;
            particleSystem.Play();
            Destroy(gameObject, 2);
            StartCoroutine(AddMoney());
        }
    }

    private IEnumerator AddMoney()
    {
        
        yield return new WaitForSeconds(0.9f);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(text.gameObject);
        MetaSceneData.Player.Money += count;
        chest.NextItem();
    }
}
