using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseWindow : OldWindow
{
    protected override Animator anim => animator;

    [SerializeField]
    private CanDisabledPopButton addHealthByCoin;

    [SerializeField]
    private PopButton addHealthByAds;

    [SerializeField]
    private PopButton exitButton;

    [SerializeField]
    private HealthShower healthShower;

    [SerializeField]
    private ButtonGuider hpGuider;
    [SerializeField]
    private ButtonGuider hpAdsGuider;

    protected Animator animator;
    private bool adsStarted;

    private void Start()
    {
        healthShower.Init();
        addHealthByCoin.Enable = (MetaSceneData.Player.HealthPoints > 0);

        if (Ads.NoAds)
            addHealthByAds.gameObject.SetActive(false);

        animator = GetComponent<Animator>();

        hpGuider.Init();
        hpAdsGuider.Init();
    }

    public override void Open()
    {
        anim.SetBool("opened", true);
        addHealthByCoin.Enable = (MetaSceneData.Player.HealthPoints > 0);
    }

    public override void OnOpen()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;

        exitButton.OnClick.AddListener(Exit);
        addHealthByAds.OnClick.AddListener(ClickToAds);
        addHealthByCoin.OnClick.AddListener(AddLifeByCoin);

        if (MetaSceneData.Player.HealthPoints > 0) {
            if (hpGuider != null) hpGuider.StartGuide();
        }
        else {
            if (hpAdsGuider != null) hpAdsGuider.StartGuide();
        }

        MetaSceneData.Win = false;
        adsStarted = false;
    }

    public override void OnClose()
    {
        if (!LevelController.Self.Win && !LevelController.Self.OpenVictoryWindow)
            Interactable.CurrentMode = Interactable.Mode.game;

        exitButton.OnClick.RemoveListener(Exit);
        addHealthByAds.OnClick.RemoveListener(ClickToAds);
        addHealthByCoin.OnClick.RemoveListener(AddLifeByCoin);

        LevelController.Self.TimePause = false;
    }

    private void AddLifeByCoin()
    {
        if (MetaSceneData.Player.HealthPoints < 1)
            Debug.LogError("HP is 0 or lower!!! By Gleb1000");

        AddLife();
    }

    private void ClickToAds()
    {
        if (!adsStarted)
            Ads.ShowVideo(AddLifeByAds, () => { adsStarted = false; }, RewardedType.addHp);
        adsStarted = true;
    }

    private void AddLifeByAds()
    {
        MetaSceneData.Player.HealthPoints++;
        MetaSceneData.Statistics.AdsHP++;
        AddLife();
    }

    protected void Exit()
    {
        if (MetaSceneData.LevelData.TimeLevel)
            SceneManager.LoadScene(MetaSceneData.GameData.CurrentLocation.Name);
        else {
            if (LevelController.Self.NotLose) {
                anim.SetBool("opened", false);
                LevelController.Self.StartCoroutine(LevelController.Self.Victory());
            }
            else SceneManager.LoadScene(MetaSceneData.GameData.CurrentLocation.Name);
        }
    }

    private void AddLife()
    {
        PlayerInfo.Self.AddLife();
        anim.SetBool("opened", false);

        MetaSceneData.Statistics.AdsHP++;

        LevelController.Self.TryWin();
    }

}
