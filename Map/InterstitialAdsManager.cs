using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterstitialAdsManager : MonoBehaviour
{
    [SerializeField]
    private int hpCountForShow;
    [SerializeField]
    private int stepForShow;

    [SerializeField]
    private ClassicChestLoot chestLoot;

    public void Init()
    {
        //if (MetaSceneDate.Player.HealthPoints > hpCountForShow)
        //    MetaSceneDate.GameData.InterstitialStep = 0;

        if ((MetaSceneData.GameData.InterstitialStep + 1) % (stepForShow + 1) == 0) {
            MetaSceneData.GameData.InterstitialStep = 0;
            MetaSceneData.InShop = false;
            Debug.Log("start interstitial");
            Ads.ShowVideo(GivePrize, () => { }, RewardedType.interstitial);
        }
    }

    public void GivePrize()
    {
        MetaSceneData.ChestLoot = chestLoot;
        SceneManager.LoadScene(MetaSceneData.chest_scene_name);
    }
}
