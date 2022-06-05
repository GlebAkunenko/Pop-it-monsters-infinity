using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class CurrentPoint : Interactable
{
    public static CurrentPoint Self { private set; get; }
    
    public static float PosY
    {
        get { return Self.transform.localPosition.y; }
    }
    
    [SerializeField]
    private int startHealth;
    [SerializeField]
    private HealthShower healthShower;
    [SerializeField]
    private TextMeshProUGUI stars;
    [SerializeField]
    private InterstitialAdsManager interstitialAds;
    [SerializeField]
    private new Transform camera;
    [SerializeField]
    private ChengeLocation goToNextLocation;
    [SerializeField]
    private Guider[] guiders;

    private bool moving;
    private bool ready;
    private SpriteRenderer spriteRenderer;

    private new void Start()
    {
        base.Start();

        Self = this;
        
        Interactable.CurrentMode = Mode.game;

        interstitialAds.Init();

        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (MetaSceneData.GameData.CurrentLocation.FirstOpen) {
            MetaSceneData.GameData.CurrentLocation.PointPos = transform.position;
            MetaSceneData.GameData.CurrentLocation.CamPos = camera.position;
            MetaSceneData.GameData.CurrentLocation.FirstOpen = false;
            spriteRenderer.color = Color.green;
        }

        transform.position = MetaSceneData.GameData.CurrentLocation.PointPos;
        camera.position = MetaSceneData.GameData.CurrentLocation.CamPos;
        if (MetaSceneData.Win && !MetaSceneData.OptionalLevel) {
            MetaSceneData.GameData.CurrentLocation.Moving = true;
        }

        if (MetaSceneData.Started)
            MetaSceneData.Statistics.SendDataToFirebase();

        stars.text = MetaSceneData.GameData.CurrentLocation.AllStars.ToString();

        LevelBar.Self.Init();
        ReviewButton.self.Init();
        healthShower.Init();
        foreach (Guider guider in guiders)
            guider.Init();

        StartCoroutine(Ready());
        StartCoroutine(LateUpdateCompliteLevels());

    }

    private IEnumerator LateFirstStart()
    {
        yield return new WaitForEndOfFrame();
        MetaSceneData.LevelData = CheckPoint.GetPointByLevel(1).Data;
    }

    private IEnumerator LateUpdateCompliteLevels()
    {
        yield return new WaitForEndOfFrame();
        CheckPoint.InitCheckPoints();
        LevelLock.InitLocks();

        bool locked = false;

        foreach (LevelLock _lock in LevelLock.Locks) {
            if (_lock.Id + 1 == MetaSceneData.GameData.CurrentLocation.Level) {
                locked = true;
                CheckPoint.UpdateCompliteLevels(MetaSceneData.GameData.CurrentLocation.Level);
                gameObject.SetActive(false);
                break;
            }

        }

        if (locked)
            yield break;

        if (CheckPoint.PointsCount == MetaSceneData.GameData.CurrentLocation.Level - 1) {
            CheckPoint.UpdateCompliteLevels(MetaSceneData.GameData.CurrentLocation.Level);
            EndLocation();
            yield break;
        }

        MetaSceneData.LevelData = CheckPoint.GetPointByLevel(MetaSceneData.GameData.CurrentLocation.Level).Data;
        CheckPoint.UpdateCompliteLevels(MetaSceneData.GameData.CurrentLocation.Level);

        if (MetaSceneData.GameData.CurrentLocation.Moving)
            StartCoroutine(Moving(MetaSceneData.GameData.CurrentLocation.Level));
    }

    private IEnumerator Ready()
    {
        yield return new WaitForSeconds(1f);
        ready = true;
    }


    private IEnumerator Moving(int level)
    {
        moving = true;

        CheckPoint checkPoint = CheckPoint.GetPointByLevel(level);
        Vector3 target = checkPoint.Position;

        spriteRenderer.color = Color.yellow;

        Vector3 newPos = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
        while (newPos != target) {
            transform.position = newPos;
            yield return new WaitForEndOfFrame();
            newPos = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
        }

        moving = false;
        EndMoving();
    }

    protected override void OnClick()
    {
        if (!moving && ready) {
            MetaSceneData.LevelData = CheckPoint.GetPointByLevel(MetaSceneData.GameData.CurrentLocation.Level).Data;
            MetaSceneData.Level_id = MetaSceneData.GameData.CurrentLocation.Level;
            MetaSceneData.OptionalLevel = false;
            OldLevelButton.Selected = null;
            StartLevelMenu.self.Open();
            click?.Invoke();
        }
    }
    
    public void EndMoving()
    {
        spriteRenderer.color = Color.green;
        MetaSceneData.GameData.CurrentLocation.Moving = false;
    }

    public void SaveCamPosition()
    {
        MetaSceneData.GameData.CurrentLocation.CamPos = camera.position;
    }


    public static void StartLevel()
    {
        MetaSceneData.GameData.InterstitialStep++;
        MetaSceneData.Started = true;
        MetaSceneData.GameData.CurrentLocation.PointPos = Self.transform.position;
        MetaSceneData.GameData.CurrentLocation.CamPos = Self.camera.position;
        MetaSceneData.SaveData();
        MetaSceneData.InShop = false;
        SceneManager.LoadScene(MetaSceneData.buttle_level_name);
    }

    public void SavePosition()
    {
        MetaSceneData.GameData.CurrentLocation.PointPos = transform.position;
    }

    private void EndLocation()
    {
        goToNextLocation.Open();
        gameObject.SetActive(false);
    }

    public event Action click;
}
