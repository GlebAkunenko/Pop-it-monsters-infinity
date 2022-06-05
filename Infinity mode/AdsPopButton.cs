using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsPopButton : PopButton
{
    [SerializeField]
    private RewardedType type;

    private RewardedAdBlock rewardedAd;

    private void Start()
    {
        rewardedAd = InfAds.GetBlockWithReward(type);
        if (!rewardedAd.IsReady())
            gameObject.SetActive(false);
        rewardedAd.AdsEnded += () => { gameObject.SetActive(false); };
    }
}
