using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChallengeArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Challange challange;
    [SerializeField]
    private Condition[] unlockConditions;

    [Header("Scene options")]
    [SerializeField]
    private GameObject lockLayer;
    [SerializeField]
    private GameObject unlockLayer;
    [SerializeField]
    private GameObject finishedLayer;
    [SerializeField]
    private ChallengeWindow window;

    private bool inited;

    private Savable<bool> finished;

    public bool IsUnlock { private set; get; }

    public bool IsFinished
    {
        get => finished;
        set => finished.Set(value);
    }

    public void Init(LocationChallengesData challengesData)
    {
        IsUnlock = Condition.CheckSeveralConditions(unlockConditions, challengesData);
        finished = new Savable<bool>(false, string.Format("loc{0}_ch{1}_fin", challengesData.LocationId, challange.ChallengId));

        SetUpLayerObjects();

        inited = true;
    }

    private void SetUpLayerObjects()
    {
        lockLayer.SetActive(IsUnlock && !IsFinished);
        unlockLayer.SetActive(!IsUnlock && !IsFinished);
        finishedLayer.SetActive(IsFinished);
    }

    public void OpenChallengeWindow()
    {
        window.Open(challange);
    }
}
