using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(StarBar))]
[RequireComponent(typeof(TimeCounter))]
public class ClassicLevelController : LevelController
{
    [SerializeField]
    private VictoryWindow victoryWindow;
    [SerializeField]
    private LoseWindow loseWindow;

    private StarBar starBar;
    private TimeCounter timeCounter;

    private int nextMonster = 0;
    private float time_limit = 0;

    public byte Stars { private set; get; }


    private void Start()
    {
        Self = this;

        starBar = GetComponent<StarBar>();
        starBar.Init();

        timeCounter = GetComponent<TimeCounter>();
        timeCounter.Init();

        Camera = GetComponent<Camera>();
        SceneSounds = GetComponent<AudioSource>();

        Interactable.CurrentMode = Interactable.Mode.game;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
            new Parameter(FirebaseAnalytics.ParameterLevelName, MetaSceneData.AnaliticsLevelName));

        MetaSceneData.Statistics = new LevelStatistics();

        if (MetaSceneData.LevelData.TimeLevel)
            starBar.Hide();
        else
            timeCounter.Hide();

    }

    private IEnumerator OpenLoseWindow()
    {
        yield return new WaitForSeconds(1);
        loseWindow.Open();
    }



    public override void NextMonster()
    {
        int monstersCount = MetaSceneData.LevelData.Monsters.Length;

        if (nextMonster == monstersCount) {
            Win = true;
            starBar.UpdateBar(nextMonster, monstersCount);
            if (PlayerInfo.Health > 0)
                StartCoroutine(Victory());
            return;
        }

        if (!MetaSceneData.LevelData.TimeLevel)
            starBar.UpdateBar(nextMonster, monstersCount);

        MonsterData monsterData = MetaSceneData.LevelData.Monsters[nextMonster++];

        time_limit = monsterData.Time;

        Interactable.CurrentMode = Interactable.Mode.none;

        GameObject o = Instantiate(monsterData.Monster, spawnTarget.position, Quaternion.identity);
        Monster monster = o.GetComponentInChildren<Monster>();

        monster.HealthReduced += (int health) => {
            ValueShower.Monster.ReduceValue();
            if (health == 0)
                TimePause = true;
        };
        monster.Spawned += () => { TimePause = false; };
        monster.Dead += NextMonster;

        Color monsterColor = monsterData.Color;
        //if (monsterData.AutoColor || MetaSceneDate.LevelData.AutoColored)
        //    monsterColor = monster.GetAutoColor(MetaSceneDate.Level_id - 1);
        
        // NOT USING
        // NEED TO ADAPT TO LAST VERSION

        ParticalMonster particalMonster = monster as ParticalMonster;
        if (particalMonster == null) {
            monster.Health = monsterData.Health;
            monster.Paint(monsterColor);
            ValueShower.Monster.ChangeFrame(monsterData.Health);
        }
        else {
            particalMonster.Paint(monsterColor);
            particalMonster.Health = particalMonster.MiniMonsters.Length;
            ValueShower.Monster.ChangeFrame(particalMonster.MiniMonsters.Length);
        }

        monster.transform.localScale = Vector3.zero;
        currentMonterAnim = monster.GetComponent<Animator>();
    }

    public override void StartGame()
    {
        if (MetaSceneData.LevelData.TimeLevel)
            timeCounter.Begin();
        NextMonster();
    }

    public override IEnumerator Victory()
    {
        OpenVictoryWindow = true;

        Interactable.CurrentMode = Interactable.Mode.canvas;

        if (MetaSceneData.LevelData.TimeLevel)
            timeCounter.Stop();

        pausePanel.SetActive(false);

        int time = timeCounter.Time;

        if (MetaSceneData.LevelData.TimeLevel) {
            if (time <= MetaSceneData.LevelData.ThreeStars)
                Stars = 3;
            else if (time <= MetaSceneData.LevelData.TwoStars)
                Stars = 2;
            else Stars = 1;
        }
        else {
            if (Win) Stars = 3;
            else {
                float progress = (nextMonster - 1f) / MetaSceneData.LevelData.Monsters.Length;
                progress *= 3;
                Stars = (byte)Mathf.FloorToInt(progress);
            }
        }

        yield return new WaitForSeconds(1);

        victoryWindow.Open();
    }

    public override void TryWin()
    {
        if (Win && PlayerInfo.Health > 0)
            StartCoroutine(Victory());
    }

    public override void Exit()
    {
        if (!OpenVictoryWindow && !Win) {
            if (NotLose) StartCoroutine(Victory());
            else SceneManager.LoadScene(MetaSceneData.GameData.LocationName);
        }
    }

    public override void Lose()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
        TimePause = true;

        StartCoroutine(OpenLoseWindow());
    }

}
