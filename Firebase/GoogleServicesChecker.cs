using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class GoogleServicesChecker : MonoBehaviour
{
    private FirebaseApp app;

    [SerializeField]
    private GameObject warringText;

    [SerializeField]
    private GameObject loading;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                app = Firebase.FirebaseApp.DefaultInstance;
            }
            else {
                if (warringText != null) warringText.SetActive(true);
            }
            if (loading != null) loading.SetActive(false);
        });
    }

}
