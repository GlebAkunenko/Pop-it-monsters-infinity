using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationStartController : MonoBehaviour
{
    [SerializeField]
    private BackMusic musicConteroller;
    [SerializeField]
    private string trainingSceneName = "TrainingLevel";
    [SerializeField]
    private bool skipTraining;

    private static bool spawned;

    private void Awake()
    {
        if (spawned) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        spawned = true;

        musicConteroller.Init();
        Crashlytics.Init();

        string keyName = TrainingLevelController.end_training_key_name;
        if (!DataSaver.HasKey(keyName))
            DataSaver.SetKey(keyName, Application.isEditor && skipTraining ? true : false);
        if ((bool)DataSaver.GetKey(keyName) == false)
            SceneManager.LoadScene(trainingSceneName);

    }
}
