using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryBehaviour : AsyncMouthBehaviour
{
    [SerializeField]
    private GameObject[] spikes;

    protected override void FirstEvent()
    {
        animator.SetFloat(blend_tree_parameter, 1);
        MakeMouthesAsync();        
    }

    protected override void SecondEvent()
    {
        animator.SetFloat(blend_tree_parameter, 0);
        RestartMouthes();
        foreach (GameObject g in spikes)
            g.SetActive(true);
    }
}
