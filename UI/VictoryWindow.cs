using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryWindow : OldWindow
{
    public static VictoryWindow self { private set; get; }

    protected override Animator anim => animator;

    [SerializeField]
    private Slider experienceSlider;

    [SerializeField]
    private Text level;      
    [SerializeField]
    private TextMeshProUGUI proLevel;        

    [SerializeField]
    private ChestButtonLevel silverChest;
    [SerializeField]
    private ChestButtonLevel goldChest;
    [SerializeField]
    private ChestButtonLevel optionalAdsSilverChest;
    [SerializeField]
    private PopButton exit;
    [SerializeField]
    private Animation optionalExit;    
    [SerializeField]
    private Animator[] stars;


    private Animator animator;
    private ClassicLevelController levelController;

    private bool addingExp;
    private int starsCount;
    private int endExperience;

    private void Start()
    {
        self = this;

        animator = GetComponent<Animator>();
        levelController = (ClassicLevelController)LevelController.Self;
        transform.localScale = Vector3.zero;

        experienceSlider.minValue = 0;
        experienceSlider.maxValue = MetaSceneData.Player.MaxExperience;

        ChangeLevel(MetaSceneData.Player.Level);
        ChangeExperience(MetaSceneData.Player.Experience);

        if (MetaSceneData.OptionalLevel) {
            exit.gameObject.SetActive(true);
            Destroy(silverChest.gameObject);
            Destroy(goldChest.gameObject);
        }
        else {
            exit.gameObject.SetActive(false);
            optionalAdsSilverChest.gameObject.SetActive(false);
        }
    }

    public override void Open()
    {
        anim.SetBool("opened", true);

        MetaSceneData.Player.OnExperienceChange += ChangeExperience;
        MetaSceneData.Player.OnLevelChange += ChangeLevel;

        if (!MetaSceneData.OptionalLevel) {
            MetaSceneData.GameData.CurrentLocation.Level++;
            MetaSceneData.GameData.CurrentLocation.Moving = true;
        }
    }

    public void NextStar()
    {
        if (starsCount == levelController.Stars) {
            if (!addingExp) {
                StartCoroutine(AddingExperience());
                endExperience = MetaSceneData.Player.Experience + MetaSceneData.LevelData.Experience;

                if (MetaSceneData.OptionalLevel) {
                    optionalExit.Play();
                    exit.OnClick.AddListener(() => {
                        SceneManager.LoadScene(MetaSceneData.GameData.CurrentLocation.Name);
                    });
                }
            }
            return;
        }

        stars[starsCount++].SetTrigger("Go");
    }

    public void SaveExperiance()
    {
        StopAllCoroutines();
        MetaSceneData.Player.Experience = endExperience;
        MetaSceneData.Win = true;
    }

    private void ChangeLevel(int value)
    {
        if (level != null)
            level.text = value.ToString();
        if (proLevel != null)
            proLevel.text = value.ToString();
    }

    private void ChangeExperience(int value)
    {
        experienceSlider.value = value;
    }

    public override void OnOpen()
    {
        if (!MetaSceneData.OptionalLevel) {
            goldChest.CanOpen = true;
            goldChest.Spawn();
            StartCoroutine(SilverChestSpawn());
        }
        else {
            optionalAdsSilverChest.CanOpen = true;
            optionalAdsSilverChest.Spawn();
        }
        NextStar();
    }

    private IEnumerator SilverChestSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        silverChest.Spawn();
        silverChest.CanOpen = true;
    }

    private IEnumerator AddingExperience()
    {
        addingExp = true;

        int buffer = MetaSceneData.LevelData.Experience;

        float s = buffer / 40;
        int step = Mathf.Max(1, Mathf.CeilToInt(s));

        while (buffer > 0) {
            MetaSceneData.Player.Experience += step;
            buffer -= step;
            yield return new WaitForSeconds(0.05f);
        }
        MetaSceneData.Player.Experience += buffer;

        MetaSceneData.Win = true;

        addingExp = false;
    }

    private void OnDestroy()
    {
        MetaSceneData.Player.OnLevelChange -= ChangeLevel;
        MetaSceneData.Player.OnExperienceChange -= ChangeExperience;
    }

    public override void OnClose()
    {
        throw new System.NotImplementedException();
    }
}
