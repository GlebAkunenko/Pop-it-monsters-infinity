using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InfinityLoseWindow : Window
{
    private Animator anim;

    [SerializeField]
    private AddHealthButton addHealthButton;
    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private GameObject inGamePlayersHealthShower;

    private PlayerStorageData storage;

    private void Start()
    {
        anim = GetComponent<Animator>();
        storage = PlayerStorageData.GetInstance();
        addHealthButton.Init();
        
        PlayerInfo.Self.OnGetPositiveHealth += () => {
            continueButton.SetActive(true);
        };
    }

    /// <summary>
    /// Close lose screen after heal HP and continue game
    /// </summary>
    public void Continue()
    {
        Interactable.CurrentMode = Interactable.Mode.none;  // to avoid double click on continue button
        inGamePlayersHealthShower.SetActive(true);
        Close();
    }

    public void Open()
    {
        anim.SetBool("opened", true);
        inGamePlayersHealthShower.SetActive(false);
        continueButton.SetActive(false);
        addHealthButton.UpdateState(storage.HealthCoins);
        Interactable.CurrentMode = Interactable.Mode.canvas;
    }

    public void Close()
    {
        anim.SetBool("opened", false);
    }

    // on end closing
    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
    }

    public override void OnOpen()
    {

    }
}
