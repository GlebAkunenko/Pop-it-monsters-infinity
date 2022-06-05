using GoogleMobileAds.Api;

public static class InfAds
{
    public static InterstitleAdsBlock Interstitle { private set; get; }
    public static RewardedAdBlock AddHealthBlock { private set; get; }

    public static RewardedAdBlock GetBlockWithReward(RewardedType type)
    {
        switch (type) {
            case RewardedType.addHp:
                return AddHealthBlock;
            default:
                throw new System.Exception();
        }
    }

    static InfAds()
    {
        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
        .SetMaxAdContentRating(MaxAdContentRating.PG)
        .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize((initStatus) => { });

        AddHealthBlock = new RewardedAdBlock(AdversamentSettings.revardedAddHealth, RewardedType.addHp);
        AddHealthBlock.LoadAd();
        Interstitle = new InterstitleAdsBlock(AdversamentSettings.interstitle);
        Interstitle.LoadAd();
    }
}


