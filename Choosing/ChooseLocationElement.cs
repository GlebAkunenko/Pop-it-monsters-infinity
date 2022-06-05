using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLocationElement : MonoBehaviour
{
    [SerializeField]
    private InfinityLevelData levelData;
    [SerializeField]
    private Transform buttonsRoot;

    private float minSize;
    private float maxSize;
    private float buttonsRootSpeedCoefficient;

    private bool CheckReadySize(float size)
    {
        return size > (minSize + maxSize) / 2;
    }

    private void StartLevel()
    {
        SceneTransition.AnimatedLoadScene(MetaSceneData.infinity_level_name);
    }

    private void StartLevelWithoutTime()
    {
        levelData.TimeLevel = false;
        MetaSceneData.InfinityLevelData = levelData;
        StartLevel();
    }

    private void StartLevelWithTime()
    {
        throw new NotImplementedException();
    }

    private void TrySincLevelDataProgress()
    {
        if (MetaSceneData.InfinityLevelData == null)
            return;

        if (MetaSceneData.InfinityLevelData.Id == levelData.Id)
            levelData = MetaSceneData.InfinityLevelData;
    }

    public void Init(float minSize, float maxSize, float buttonsRootSpeedCoefficient)
    {
        transform = GetComponent<Transform>();
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.buttonsRootSpeedCoefficient = buttonsRootSpeedCoefficient;

        if (!levelData.IsInited)
            levelData.Init();

        TrySincLevelDataProgress(); 
    }

    public void TransormScale(float scaleValue, float step)
    {
        float s = Mathf.SmoothStep(transform.localScale.x, scaleValue, step);
        transform.localScale = s * Vector3.one;

        float root_scale = Mathf.SmoothStep(buttonsRoot.localScale.x, s / maxSize, step * buttonsRootSpeedCoefficient);
        buttonsRoot.localScale = root_scale * Vector3.one;        
    }

    public void ClickStartLevelWithoutTimeButton()
    {
        if (PlayerInfo.Health > 0)
            StartLevelWithoutTime();
        else
            NoHealthToStartLevelWindow.self.Open();
    }



    public float PosY => transform.position.y;
    public new Transform transform { get; private set; }
}
