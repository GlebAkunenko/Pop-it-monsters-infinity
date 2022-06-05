using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingLevelController : LevelController
{
    public const string end_training_key_name = "training_end";

    private static SavableInt offset;

    [Header("Settings")]
    [SerializeField]
    private string trainingSceneName = "TrainingLevel";
    [SerializeField]
    private string chooseLocationSceneName = "ChooseLocation";
    [SerializeField]
    private PageCollection firstCollection;
    [SerializeField]
    private GameObject[] firstMonsters;
    [SerializeField]
    private GameObject firstChest;
    [SerializeField]
    private GameObject[] secondMonsters;
    [SerializeField]
    private GameObject secondChest;

    [Header("Components")]
    [SerializeField]
    private InfinityPlayerInfo playerInfo;
    [SerializeField]
    private InfinityBar bar;
    [SerializeField]
    private InfinityLoseWindow loseWindow;

    private bool firstMonster = true;
    private TrainingStack stack;

    private void Awake()
    {
        Self = this;
        offset = new SavableInt(0, "training_offset");
    }

    private void Start()
    {
        bar.Init();

        playerInfo.Init();
        PlayerInfo.Self.OnZeroHealth += Lose;

        List<IBarElement> elements = new List<IBarElement>();
        foreach (GameObject m in firstMonsters)
            elements.Add(m.GetComponentInChildren<Monster>());
        elements.Add(firstChest.GetComponent<TrainingInfChestLoot>());
        foreach (GameObject m in secondMonsters)
            elements.Add(m.GetComponentInChildren<Monster>());
        elements.Add(secondChest.GetComponent<TrainingInfChestLoot>());
        stack = new TrainingStack(elements, offset);

        bar.DrawBar(stack.CurrentStack);
    }

    private IEnumerator OpenLoseWindow()
    {
        yield return new WaitForSeconds(1);
        loseWindow.Open();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Lose()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
        StartCoroutine(OpenLoseWindow());
    }

    public override void NextMonster()
    {
        Interactable.CurrentMode = Interactable.Mode.none;

        if (!firstMonster)
            bar.PushBar(stack.CurrentStack);
        firstMonster = false;

        IBarElement newElement = stack.PopElementAndPushStack();

        Monster monster = newElement as Monster;
        if (monster != null) {

            GameObject instance = Instantiate(monster.transform.parent.gameObject, spawnTarget.position, Quaternion.identity);
            monster = instance.GetComponentInChildren<Monster>();
            monster.transform.localScale = Vector3.zero;
            currentMonterAnim = monster.GetComponent<Animator>();
            instance.GetComponentInChildren<Animator>().SetBool("training", true);
            ValueShower.Monster.ChangeFrame(monster.Health);

            if (monster.Health > 1) {
                monster.HealthReduced += (int health) => {
                    ValueShower.Monster.ReduceValue();
                    if (health == 0)
                        TimePause = true;
                };
            }
            monster.Dead += () => {
                offset++;
                NextMonster();
            };

        }
        else {
            offset++;
            if (stack.CurrentStack.Count == 0) {
                DataSaver.SetKey(end_training_key_name, true);
                DataSaver.SaveData();
            }

            SceneTransition.LoadOpenChestScene((IChestInfo)newElement, firstCollection, true,
                stack.CurrentStack.Count > 0 ? trainingSceneName : chooseLocationSceneName);
        }
    }

    public override void StartGame()
    {
        NextMonster();
    }

    public override void TryWin()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Victory()
    {
        throw new System.NotImplementedException();
    }
}
