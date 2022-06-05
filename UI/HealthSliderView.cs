using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSliderView : MonoBehaviour
{
    [SerializeField]
    private MySlider slidersController;
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    [SerializeField]
    private GameObject[] healthFrames;

    private PlayerStorageData storage;

    private void Start()
    {
        storage = PlayerStorageData.GetInstance();
        foreach (GameObject f in healthFrames)
            f.SetActive(false);
        slidersController.Init(Mathf.Min(storage.HealthCoins, 5));
        slidersController.ValueChanged += OnSliderChange;
    }

    private void OnSliderChange(int value)
    {
        ChangeCoinsCount(storage.HealthCoins - value);
        for (int i = 0; i < healthFrames.Length; i++)
            healthFrames[i].SetActive(i + 1 == value);
    }

    private void ChangeCoinsCount(int count)
    {
        text.text = count.ToString();
    }



    
}
