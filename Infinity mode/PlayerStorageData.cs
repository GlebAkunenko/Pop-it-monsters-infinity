using System;

public class PlayerStorageData : IAutoSaving
{
    private static PlayerStorageData instance;

    private PlayerStorageData() 
    {
        DataSaver.AddAutoSaveObject(this);
        if (DataSaver.HasKey(KeyName)) {
            SerealizationData data = (SerealizationData)DataSaver.GetKey(KeyName);
            HealthCoins = data.HealthCoins;
            Money = data.Money;
        }
    }

    public static PlayerStorageData GetInstance()
    {
        if (instance == null)
            instance = new PlayerStorageData();
        return instance;
    }

    private int healthCoins;
    private int money;

    public int HealthCoins
    {
        get => healthCoins;
        set {
            healthCoins = value;
            HealthCoinsChange?.Invoke(healthCoins);
        }
    }
    public int Money
    {
        get => money;
        set {
            money = value;
            MoneyChange?.Invoke(money);
        }
    }

    public string KeyName => "storage_data";

    public object GetSavingData()
    {
        return new SerealizationData() {
            HealthCoins = HealthCoins,
            Money = Money
        };
    }

    [Serializable]
    private struct SerealizationData
    {
        public int HealthCoins;
        public int Money;
    }

    public event Action<int> HealthCoinsChange;
    public event Action<int> MoneyChange;
}