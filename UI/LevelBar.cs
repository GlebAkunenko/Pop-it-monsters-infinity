using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelBar : MonoBehaviour
{
    public static LevelBar Self { get; private set; }

    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private Slider experience;

    private void Awake()
    {
        Self = this;
    }

    public void Init()
    {
        MetaSceneData.Player.OnExperienceChange += ChangeExperience;
        MetaSceneData.Player.OnLevelChange += ChangeLevel;

        ChangeExperience(MetaSceneData.Player.Experience);
        ChangeLevel(MetaSceneData.Player.Level);
    }

    private void ChangeExperience(int value)
    {
        experience.maxValue = MetaSceneData.Player.MaxExperience;
        experience.value = value;
    }

    private void ChangeLevel(int value)
    {
        level.text = value.ToString();
        experience.maxValue = MetaSceneData.Player.MaxExperience;
    }

    private void OnDestroy()
    {
        MetaSceneData.Player.OnLevelChange -= ChangeLevel;
        MetaSceneData.Player.OnExperienceChange -= ChangeExperience;
    }
}
