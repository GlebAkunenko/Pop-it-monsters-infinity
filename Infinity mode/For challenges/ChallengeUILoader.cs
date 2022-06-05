using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeUILoader : MonoBehaviour
{
    private int playerHealth;
    [SerializeField]
    private InfinityBar bar;
    [SerializeField]
    private Animation countdown;

    private bool inited;

    public void Init(int playerHealth)
    {
        this.playerHealth = playerHealth;
        inited = true;
        GetComponent<Animation>().Play();
    }

    public void SpawnPlayersHealth()
    {
        if (!inited)
            throw new System.Exception("UI loader not inited!");

        foreach (ValueShower player in ValueShower.Players)
            player.UpdateValue(playerHealth);
    }

    public void StartBarAnimation()
    {
        if (!inited)
            throw new System.Exception("UI loader not inited!");

        bar.AnimateSpawnBlocks();
    }

    public void StartCountDown()
    {
        if (!inited)
            throw new System.Exception("UI loader not inited!");

        countdown.Play();
    }
}
