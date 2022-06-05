using UnityEngine;

public class ClassicPlayerInfo : PlayerInfo
{
    public override int HealthMax => ValueShower.player_hp_max_count;

    private void Start()
    {
        Self = this;

        if (MetaSceneData.Player.HealthPoints > 0)
            Health = Mathf.Min(ValueShower.player_hp_max_count, MetaSceneData.Player.HealthPoints);
        else
            Health = 1;

        foreach (ValueShower shower in ValueShower.Players)
            shower.UpdateValue(Health);
    }

    public override void GetDamage()
    {
        foreach (ValueShower shower in ValueShower.Players)
            shower.ReduceValue();
        Health--;
        MetaSceneData.Statistics.HpDelta--;
        MetaSceneData.Statistics.LoseHP++;

        if (MetaSceneData.Player.HealthPoints > 0)
            MetaSceneData.Player.HealthPoints--;

        if (Health == 0)
            CallZeroHealthEvent();
    }

    public override void AddLife()
    {
        foreach (ValueShower shower in ValueShower.Players)
            shower.UpdateValue(1);
        Health = 1;
    }
}
