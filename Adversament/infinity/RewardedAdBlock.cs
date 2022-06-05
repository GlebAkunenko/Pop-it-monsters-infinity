using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class RewardedAdBlock
{
    const string testRewardedId = "ca-app-pub-3940256099942544/5224354917";

    private string unitId;
    private RewardedType type;
    private RewardedAd rewardedAd;

    public RewardedType Type => type;

    private Action ResultAction;

    public RewardedAdBlock(string unitId, RewardedType type)
    {
        this.unitId = Application.isEditor ? testRewardedId : unitId;
        this.type = type;
    }

    public void LoadAd()
    {
        rewardedAd = new RewardedAd(unitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnAdOpening += OnOpen;
        rewardedAd.OnAdClosed += OnClose;
        rewardedAd.OnUserEarnedReward += OnSuccess;
        rewardedAd.OnAdFailedToLoad += OnAdsEnd;
    }

    private void OnAdsEnd(object sender, AdFailedToLoadEventArgs e)
    {
        AdsEnded?.Invoke();
    }

    private void OnSuccess(object sender, Reward e)
    {
        ResultAction();
    }

    private void OnClose(object sender, System.EventArgs e)
    {
        BackMusic.self.UnmuteCourseAds();
        LoadAd();
    }

    private void OnOpen(object sender, System.EventArgs e)
    {
        BackMusic.self.MuteCourseAds();
    }

    public bool IsReady()
    {
        return rewardedAd.IsLoaded();
    }

    public void ShowAds(Action successAction)
    {
        ResultAction = successAction;
        rewardedAd.Show();
    }

    public event Action AdsEnded;

}


