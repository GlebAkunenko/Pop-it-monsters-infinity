using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationQuiteController : MonoBehaviour
{
    private static bool spawned;

    private void Awake()
    {
        if (spawned) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        spawned = true;
    }

    private void OnApplicationQuit()
    {
        DataSaver.AutoSave();
        DataSaver.WriteDataOnDrive();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            DataSaver.SaveData();
    }
}
