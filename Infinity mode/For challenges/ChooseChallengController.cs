using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseChallengController : MonoBehaviour
{
    [SerializeField]
    private string menuSceneName = "ChooseLocation";
    [SerializeField]
    private ChallengeArea[] challenges;

    private LocationChallengesData challengesData;

    private void Start()
    {
        challengesData = MetaSceneData.ChallengesData;
        foreach (ChallengeArea challenge in challenges)
            challenge.Init(challengesData);
    }

    public void Leave()
    {
        SceneTransition.AnimatedLoadScene(menuSceneName);
    }
}
