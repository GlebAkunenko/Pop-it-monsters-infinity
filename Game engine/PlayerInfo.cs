using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInfo : MonoBehaviour
{  
    public static PlayerInfo Self { protected set; get; }

    public static int Health { get; protected set; } = -1;

    public abstract int HealthMax { get; }

    public abstract void GetDamage();

    public abstract void AddLife();

    protected void CallZeroHealthEvent()
    {
        OnZeroHealth?.Invoke();
    }

    protected void CallGetPositiveHealth()
    {
        OnGetPositiveHealth?.Invoke();
    }

    public event System.Action OnZeroHealth;
    public event System.Action OnGetPositiveHealth;
}
