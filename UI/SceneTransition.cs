using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private const string chestOpeningSceneName = "ChestOpening";

    private static SceneTransition self;

    private Animator animator;

    private string loadSceneName = "";

    private void Start()
    {
        self = this;
        animator = GetComponent<Animator>();
    }

    public void OnEndClosing()
    {
        if (loadSceneName != "")
            SceneManager.LoadScene(loadSceneName);
    }

    public static void AnimatedLoadScene(string sceneName)
    {
        self.loadSceneName = sceneName;
        self.animator.SetTrigger("close");
    }

    /// <summary>
    /// Animated load scene to chestOpeningScene and set this settings
    /// </summary>
    /// <param name="chest">what will be opened</param>
    /// <param name="backSceneName">if not empty there will be a button to this scene</param>
    public static void LoadOpenChestScene(IChestInfo chest, PageCollection collection, bool trainingMode, string backSceneName="")
    {
        MetaSceneData.ChestLoot = chest;
        MetaSceneData.ChestLootCollection = collection;
        MetaSceneData.ChestOpeningBackSceneName = backSceneName;
        MetaSceneData.TrainingMode = trainingMode;
        AnimatedLoadScene(chestOpeningSceneName);
    }
}
