using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private string[] locationsName;

    [SerializeField]
    private BackMusic backMusic;
    [SerializeField]
    private Ads adsManager;

    private void Start()
    {
        MetaSceneData.LoadData(locationsName);
        backMusic.Init();
        adsManager.Init();
    }

    public void LoadMap()
    {
        if (MetaSceneData.GameData.CurrentLocation != null)
            SceneManager.LoadScene(MetaSceneData.GameData.CurrentLocation.Name);
        else SceneManager.LoadScene(MetaSceneData.first_level_scene_name);
    }
}
