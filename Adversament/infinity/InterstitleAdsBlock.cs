using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitleAdsBlock
{
    const string testId = "ca-app-pub-3940256099942544/5224354917";

    private string unitId;
    private InterstitialAd interstitial;

    public InterstitleAdsBlock(string unitId)
    {
        this.unitId = Application.isEditor ? testId : unitId;
    }

    public void LoadAd()
    {
        interstitial = new InterstitialAd(unitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
        interstitial.OnAdOpening += StartAd;
    }

    private void StartAd(object sender, System.EventArgs e)
    {
        InfInterstitle.Step++;
    }

    public bool IsReady()
    {
        return interstitial.IsLoaded();
    }

    public void ShowAds()
    {
        interstitial.Show();
    }
}
