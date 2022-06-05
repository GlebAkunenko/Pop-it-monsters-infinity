using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToShopButton : PopButton
{
    private void Start()
    {
        OnClick.AddListener(GoToShop);
    }

    private void GoToShop()
    {
        CurrentPoint.Self.SavePosition();
        CurrentPoint.Self.SaveCamPosition();
        MetaSceneData.SaveData();
        SceneManager.LoadScene(MetaSceneData.shop_scene_name);
    }
}
