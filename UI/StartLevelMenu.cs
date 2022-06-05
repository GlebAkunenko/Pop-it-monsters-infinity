using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartLevelMenu : OldWindow
{
    public static StartLevelMenu self { private set; get; }

    protected override Animator anim => animator;

    [SerializeField]
    private TextMeshProUGUI twoStarsTime;
    [SerializeField]
    private TextMeshProUGUI threeStarsTime;
    [SerializeField]
    private TextMeshProUGUI monstersCount;
    [SerializeField]
    private TextMeshProUGUI bigStar;
    [SerializeField]
    private GameObject mainLevelLoot;
    [SerializeField]
    private GameObject optionalLevelLoot;
    [SerializeField]
    private ParticleSystem hpParticls;
    [SerializeField]
    private HealthShower healthShower;
    [SerializeField]
    private GameObject timers;
    [SerializeField]
    private GameObject monsterShadows;

    private Animator animator;

    private bool opened;

    private void Start()
    {
        self = this;
        animator = GetComponent<Animator>();

    }

    public override void Open()
    {
        monstersCount.text = MetaSceneData.LevelData.Monsters.Length.ToString();

        int num = MetaSceneData.Level_id;
        bigStar.text = num.ToString();

        if (MetaSceneData.LevelData.TimeLevel) {
            int two = MetaSceneData.LevelData.TwoStars / 10;
            int three = MetaSceneData.LevelData.ThreeStars / 10;
            twoStarsTime.text = two.ToString() + ".0";
            threeStarsTime.text = three.ToString() + ".0";
            timers.SetActive(true);
            monsterShadows.SetActive(false);
        }
        else {
            float n = MetaSceneData.LevelData.Monsters.Length / 3f;
            int one = Mathf.CeilToInt(n);
            int two = Mathf.CeilToInt(n * 2);

            if (one == two)
                one = 0;
            if (two == MetaSceneData.LevelData.Monsters.Length)
                two = 0;

            twoStarsTime.text = one != 0 ? one.ToString() : "-";
            threeStarsTime.text = two != 0 ? two.ToString() : "-";
            timers.SetActive(false);
            monsterShadows.SetActive(true);
        }

        mainLevelLoot.SetActive(MetaSceneData.OptionalLevel == false);
        optionalLevelLoot.SetActive(MetaSceneData.OptionalLevel == true);

        Interactable.CurrentMode = Interactable.Mode.canvas;

        anim.SetBool("opened", true);
    }

    public override void Close()
    {
        anim.SetBool("opened", false);
    }

    public void OnStartLevelButton()
    {
        Interactable.CurrentMode = Interactable.Mode.none;

        healthShower.ChangeHP(Mathf.Max(MetaSceneData.Player.HealthPoints - 3, 0));

        hpParticls.emission.SetBurst(0, 
            new ParticleSystem.Burst(0, Mathf.Min(3, MetaSceneData.Player.HealthPoints)));
        hpParticls.Play();
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        while (hpParticls.isPlaying)
            yield return new WaitForEndOfFrame();

        if (MetaSceneData.LevelData != null) {
            CurrentPoint.StartLevel();
        }
        else Debug.LogError("LevelData is null! By Gleb1000");
    }
    

    public override void OnOpen()
    {
        opened = true;
    }

    public override void OnClose()
    {
        opened = false;
        Interactable.CurrentMode = Interactable.Mode.game;
    }
}
