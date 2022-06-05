using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    #region Use only in classic (old) version

    public int AmountDamage { get; set; }

    private int money;
    /// <summary>
    /// Use only in classic (old) version
    /// </summary>
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            OnMoneyChange?.Invoke(Money);
        }
    }

    private int experience;
    /// <summary>
    /// Use only in classic (old) version
    /// </summary>
    public int Experience
    {
        get { return experience; }
        set
        {
            experience = value;
            while (experience >= MaxExperience) {
                experience -= MaxExperience;
                Level++;
            }
            OnExperienceChange?.Invoke(Experience);
        }
    }

    private int level;
    /// <summary>
    /// Use only in classic (old) version
    /// </summary>
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            OnLevelChange?.Invoke(Level);
        }
    }

    private int healthPoints;
    /// <summary>
    /// Use only in classic (old) version
    /// </summary>
    public int HealthPoints
    {
        get { return healthPoints; }
        set
        {
            healthPoints = value;
            OnHealthPointsChange?.Invoke(healthPoints);
        }
    }

    #endregion

    public int Health { get; set; }

    public static PlayerStats Empty
    {
        get
        {
            PlayerStats ret = new PlayerStats(
                level: 1,
                experience: 0,
                money: 0,
                healthPoints: 8
                );
            return ret;
        }
    }

    public PlayerStats(int level, int experience, int money, int healthPoints)
    {
        this.level = level;
        this.experience = experience;
        this.money = money;
        this.healthPoints = healthPoints;
        this.AmountDamage = 0;
    }

    [field: NonSerialized]
    public event Action<int> OnLevelChange;
    [field: NonSerialized]
    public event Action<int> OnExperienceChange;
    [field: NonSerialized]
    public event Action<int> OnHealthPointsChange;
    [field: NonSerialized]
    public event Action<int> OnMoneyChange;


    public int MaxExperience
    {
        get { return Level * 50; }
    }

}