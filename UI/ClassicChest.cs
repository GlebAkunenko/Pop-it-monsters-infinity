using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class ClassicChest : Popit
{
    public static ClassicChest CurrentChest { get; set; }

    [SerializeField]
    private Type model;

    [SerializeField]
    private PopButton exitButton;
    [SerializeField]
    private Animation exitAnim;

    [SerializeField]
    private Transform itemSpawnTarget;

    [SerializeField]
    private GameObject moneyPrefab;

    private List<GameObject> dropItems;
    private int itemIndex;
    private int moneyCount;
    private Animator anim;

    private void Start()
    {
        ClassicChestLoot chestLoot = (ClassicChestLoot)MetaSceneData.ChestLoot;

        if (chestLoot.ChestModel != model) {
            Destroy(gameObject);
            return;
        }

        CurrentChest = this;
        Interactable.CurrentMode = Interactable.Mode.game;

        Pimple.Camera = Camera.main;


        anim = GetComponent<Animator>();

        anim.SetInteger("Special", (int)chestLoot.SpecialAnimationIndex);
        anim.SetBool("Gold", chestLoot.GoldAnimation);
        anim.SetInteger("Location", chestLoot.LocationIndex);

        moneyCount = chestLoot.GenerateRandomMoney();
        dropItems = chestLoot.Unpack().lootObjects;
    }

    protected override void OnFull()
    {
        UpdatePimples();
        anim.SetTrigger("Open");
    }

    public void Open()
    {
        anim.speed = 0;
        SpawnMoney();
    }

    public void SpawnMoney()
    {
        if (moneyCount == 0)
            NextItem();
        else {
            GameObject o = Instantiate(moneyPrefab, itemSpawnTarget.position, Quaternion.identity);
            DropMoney dropMoney = o.GetComponent<DropMoney>();
            dropMoney.Init(moneyCount, this);
        }
    }


    public void NextItem()
    {
        if (itemIndex < dropItems.Count) {
            Instantiate(dropItems[itemIndex++], itemSpawnTarget.position, Quaternion.identity);
        }
        else {
            exitAnim.Play();
            exitButton.OnClick.AddListener(() => {
                MetaSceneData.SaveData();
                SceneManager.LoadScene(
                    MetaSceneData.InShop ? MetaSceneData.shop_scene_name : MetaSceneData.GameData.CurrentLocation.Name);
            });
        }
    }

    public enum Type
    {
        silver,
        gold,
        wildSilver,
        wildGold,
        minersSilver,
        minersGold,
        gift = 99
    }

}

public enum SpecialChestAnimation
{
    none = 0,
    gift = 1
}
