using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InfinityPlayerInfo : PlayerInfo, IAutoSaving
{
    private const string data_key_name = "player_health_info";

    private static bool dataLoaded;
    private static DateTime lastHealTime;

    [SerializeField]
    private int healIntervalInMinutes;
    private int healInterval => healIntervalInMinutes * 60;
    [SerializeField]
    private UnityEvent<int> TimerChangeValue;
    [SerializeField]
    private bool initOnStart;

    public string KeyName => data_key_name;

    public override int HealthMax => ValueShower.player_infinity_hp_max_count;

    private void Start()
    {
        if (initOnStart)
            Init();
    }

    public void Init()
    {
        Self = this;
        DataSaver.AddAutoSaveObject(this);

        if (!dataLoaded) {
            if (DataSaver.HasKey(data_key_name)) {
                SerealizationData data = (SerealizationData)DataSaver.GetKey(data_key_name);
                Health = data.health;
                lastHealTime = data.lastHealTime;
            }
            else {
                Health = ValueShower.player_infinity_hp_max_count;
                lastHealTime = DateTime.Now;
            }
            dataLoaded = true;
        }

        int pastTime = (int)(DateTime.Now - lastHealTime).TotalSeconds;
        if (pastTime / healInterval > 0) {
            Health = Mathf.Min(Health + pastTime / healInterval, ValueShower.player_infinity_hp_max_count);
            lastHealTime = lastHealTime.AddSeconds(pastTime % healInterval);
        }
        StartCoroutine(Timer());

        foreach(ValueShower shower in ValueShower.Players)
            shower.UpdateValue(Health);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        DataSaver.RemoveAutoSaveObject(this);
    }

    private IEnumerator Timer()
    {
        while(true) 
        {
            int seconds = (int)(DateTime.Now - lastHealTime).TotalSeconds;
            TimerChangeValue?.Invoke(healIntervalInMinutes * 60 - seconds);

            if (seconds >= healIntervalInMinutes * 60) {
                lastHealTime = DateTime.Now;
                TryHeal();
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void AddHealth()
    {
        Health++;
        if (Health == 1)
            CallGetPositiveHealth();

        foreach (ValueShower shower in ValueShower.Players)
            shower.UpdateValue(Health);
    }

    private void TryHeal()
    {
        if (Health == ValueShower.player_infinity_hp_max_count)
            return;
        AddHealth();
    }

    public override void GetDamage()
    {
        foreach (ValueShower shower in ValueShower.Players)
            shower.ReduceValue();
        Health--;
        if (Health == 0)
            CallZeroHealthEvent();
    }

    public override void AddLife()
    {
        if (Health >= ValueShower.player_infinity_hp_max_count)
            throw new Exception("Try to heal full hp player");
        AddHealth();
    }

    public object GetSavingData()
    {
        return new SerealizationData() {
            health = Health,
            lastHealTime = lastHealTime
        };
    }

    [Serializable]
    private struct SerealizationData
    {
        public int health;
        public DateTime lastHealTime;
    }
}
