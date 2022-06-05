using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpeningController : MonoBehaviour
{    
    [SerializeField]
    private InfinityChest infinityChest;
    [SerializeField]
    private PopButton continueButton;
    [SerializeField]
    private PopButton exitButton;

    private void Start()
    {
        Interactable.CurrentMode = Interactable.Mode.game;

        infinityChest.Init(MetaSceneData.ChestLoot, MetaSceneData.ChestLootCollection);
        infinityChest.Finished += OnFinishLooting;
    }

    private void OnFinishLooting()
    {
        DataSaver.SaveData();
        if (MetaSceneData.ChestOpeningBackSceneName != "") {
            continueButton.gameObject.SetActive(true);
            continueButton.OnClick.AddListener(() => {
                LeaveScene(MetaSceneData.ChestOpeningBackSceneName);
            });
        }
        if (!MetaSceneData.TrainingMode)
            exitButton.gameObject.SetActive(true);
    }

    public void LeaveScene(string newSceneName)
    {
        SceneTransition.AnimatedLoadScene(newSceneName);
    }
}
