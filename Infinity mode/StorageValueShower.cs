using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageValueShower : MonoBehaviour
{
    [SerializeField]
    private Value valueToShow;
    [SerializeField]
    private bool initOnStart;
    [SerializeField]
    private TextMeshProUGUI text;

    private PlayerStorageData storage;

    private void Start()
    {
        if (initOnStart)
            Init();
    }

    public void Init()
    {
        storage = PlayerStorageData.GetInstance();
        switch (valueToShow) {
            case Value.healthCoins:
                storage.HealthCoinsChange += Change;
                Change(storage.HealthCoins);
                break;
            case Value.money:
                storage.MoneyChange += Change;
                Change(storage.Money);
                break;
        }
    }

    public void Change(int value)
    {
        text.text = value.ToString();
    }

    private void OnDestroy()
    {
        switch (valueToShow) {
            case Value.healthCoins:
                storage.HealthCoinsChange -= Change;
                break;
            case Value.money:
                storage.MoneyChange -= Change;
                break;
        }
    }

    public enum Value
    {
        healthCoins,
        money
    }

}
