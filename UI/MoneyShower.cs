using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyShower : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private bool initOnStart;

    private void Start()
    {
        if (initOnStart)
            Init();
    }

    public void Init()
    {
        MetaSceneData.Player.OnMoneyChange += Change;
        Change(MetaSceneData.Player.Money);
    }

    public void Change(int value)
    {
        text.text = value.ToString();
    }

    private void OnDestroy()
    {
        MetaSceneData.Player.OnMoneyChange -= Change;
    }
}
