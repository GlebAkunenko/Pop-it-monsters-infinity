using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BuyWindow : OldWindow
{
    public static BuyWindow self { get; private set; }

    protected override Animator anim => animator;

    [SerializeField]
    private PopButton buyButton;

    [SerializeField]
    private Transform spawnPreviewProductsTarget;

    [SerializeField]
    private TextMeshProUGUI coinsText;

    private GameObject previewProduct;
    private new Transform transform;
    private Animator animator;

    private void Start()
    {
        self = this;
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        coinsText.text = MetaSceneData.Player.Money.ToString();
    }

    public void Open(Product product)
    {
        buyButton.OnClick.RemoveAllListeners();

        previewProduct = Instantiate(product.DisplayPrefab, spawnPreviewProductsTarget.position, Quaternion.identity, spawnPreviewProductsTarget);
        previewProduct.transform.localScale = new Vector3(1, 1, 1);

        if (MetaSceneData.Player.Money < product.Cost)
            buyButton.gameObject.SetActive(false);
        else {
            buyButton.gameObject.SetActive(true);
            buyButton.OnClick.AddListener( () => {
                MetaSceneData.Player.Money -= product.Cost;
                coinsText.text = MetaSceneData.Player.Money.ToString();
                product.GiveProduct();
            });
        }

        Interactable.CurrentMode = Interactable.Mode.canvas;
        anim.SetBool("opened", true);
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
        Destroy(previewProduct);
    }

    public override void OnOpen()
    {

    }

    public void ExitFromShop()
    {
        MetaSceneData.SaveData();
        SceneManager.LoadScene(MetaSceneData.GameData.LocationName);
    }
}
