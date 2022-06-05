using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfinityLevelController : LevelController
{
    [SerializeField]
    private string levelSceneName = "InfinityLevel";
    [SerializeField]
    private string exitSceneName = "ChooseLocation";
    [SerializeField]
    private string chestOpeningSceneName = "ChestOpening";
    [SerializeField]
    protected int barSize;
    [SerializeField]
    private ChestStep chestStep;
    [SerializeField]
    private InfChestLoot priceChest;

    [SerializeField]
    protected InfinityBar bar;
    [SerializeField]
    private InfinityLoseWindow loseWindow;
    [SerializeField]
    private InfinityPlayerInfo playerInfo;
    [SerializeField]
    protected SpriteRenderer background;

    private InfinityLevelStack stack;
    protected MonsterColorBuilder colorBuilder;

    private bool firstMonster = true;

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        Self = this;

        bar.Init();

        playerInfo.Init();
        PlayerInfo.Self.OnZeroHealth += Lose;

        InfinityLevelData levelData = MetaSceneData.InfinityLevelData;
        if (!levelData.MonstersCollection.Loaded)
            levelData.MonstersCollection.LoadUnlockedMonsters();

        MonstersStack monstersStack = new MonstersStack(levelData.GetAllUnlockedMonsters(), levelData.Id, levelData.Offset, barSize);
        monstersStack.MakeStack();
        ConstantChestStack chestStack = new ConstantChestStack(priceChest);
        colorBuilder = new MonsterColorBuilder(levelData.LastMonsterColorType, levelData.Offset);
        stack = new InfinityLevelStack(monstersStack, colorBuilder.Copy(), chestStack, levelData.Offset, chestStep, barSize);
        stack.MakeStack();

        background.sprite = levelData.PopBackground();

        bar.DrawBar(stack.CurrentStack);
    }

    private IEnumerator OpenLoseWindow()
    {
        yield return new WaitForSeconds(1);
        loseWindow.Open();
    }

    private Monster SpawnMonster(Monster prefabMonsterComponent)
    {
        GameObject instance = Instantiate(prefabMonsterComponent.transform.parent.gameObject, spawnTarget.position, Quaternion.identity);
        prefabMonsterComponent = instance.GetComponentInChildren<Monster>();
        prefabMonsterComponent.transform.localScale = Vector3.zero;
        currentMonterAnim = prefabMonsterComponent.GetComponent<Animator>();
        return prefabMonsterComponent;
    }

    protected virtual void SetUpMonster(Monster monster, Color color)
    {
        if (monster.Health > 1) {
            monster.HealthReduced += (int health) => {
                ValueShower.Monster.ReduceValue();
                if (health == 0)
                    TimePause = true;
            };
        }
        monster.Spawned += () => {
            if (Interactable.CurrentMode != Interactable.Mode.pause)
                Interactable.CurrentMode = Interactable.Mode.game;
            TimePause = false;
        };
        monster.Dead += () => OnMosterDead();

        ParticalMonster particalMonster = monster as ParticalMonster;
        if (particalMonster == null) {
            monster.Paint(color);
            ValueShower.Monster.ChangeFrame(monster.Health);
        }
        else {
            particalMonster.Paint(color);
            particalMonster.Health = particalMonster.MiniMonsters.Length;
            ValueShower.Monster.ChangeFrame(particalMonster.MiniMonsters.Length);
        }
    }

    protected virtual void OnMosterDead()
    {
        InfInterstitle.Step++;
        MetaSceneData.InfinityLevelData.Offset++;
        NextMonster();
    }

    private void DropChest(IChestInfo newElement)
    {
        MetaSceneData.InfinityLevelData.Offset++;
        MetaSceneData.InfinityLevelData.OpenedChests++;
        FirebaseLogEvent.OpenLocationChest(MetaSceneData.InfinityLevelData.Id);
        SceneTransition.LoadOpenChestScene(newElement,
            MetaSceneData.InfinityLevelData.MonstersCollection, false, levelSceneName);
    }

    public override void Exit()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
        SceneTransition.AnimatedLoadScene(exitSceneName);
    }

    public override void Lose()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
        TimePause = true;

        //firebase.LogLose();

        StartCoroutine(OpenLoseWindow());
    }

    public override void NextMonster()
    {
        Interactable.CurrentMode = Interactable.Mode.none;

        if (!firstMonster)
            bar.PushBar(stack.CurrentStack);
        firstMonster = false;

        IBarElement newElement = stack.PopElementAndPushStack();

        if (newElement is MonsterBarElement) {
            MonsterBarElement monsterBarElement = (MonsterBarElement)newElement;
            SetUpMonster(SpawnMonster(monsterBarElement.Monster), monsterBarElement.BarIconColor);
        }
        else
            DropChest((IChestInfo)newElement);

    }

    public override void StartGame()
    {
        //if (MetaSceneDate.InfinityLevelData.TimeLevel)
        //    timeCounter.Begin();
        NextMonster();
    }

    public override void TryWin()
    {
        ;
    }

    public override IEnumerator Victory()
    {
        throw new System.NotImplementedException();
    }
}
