using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeWindow : Window
{
    [SerializeField]
    private string challengeSceneName = "ChallengeLevel";
    [SerializeField]
    private MySlider healthSlider;

    private Challange currentChallenge;
    private bool startChallengeOnClose;

    private int SliderHealth { get; set; }

    private void Start()
    {
        healthSlider.ValueChanged += (int v) => SliderHealth = v;
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
        if (startChallengeOnClose) {
            MetaSceneData.Challange = currentChallenge;
            SceneTransition.AnimatedLoadScene(challengeSceneName);
        }
    }

    public void Open(Challange challange)
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
        currentChallenge = challange;
    }

    public void StartChallenge()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        startChallengeOnClose = true;
        currentChallenge.PlayerHealth = SliderHealth;
        Close();
    }
}
