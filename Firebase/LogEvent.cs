using Firebase.Analytics;

/// <summary>
/// Not use
/// </summary>
public class LogEvent
{
    private string firebaseName;
    private string paramName;
    private int value;

    private LogEvent(string firebaseName, string paramName, int value)
    {
        this.firebaseName = firebaseName;
        this.paramName = paramName;
        this.value = value;
    }

    public static LogEvent DropMonster(Rarity rarity)
    {
        return new LogEvent("drop_monster", "rarity", (int)rarity);
    }

    public static LogEvent OpenLocationChest(int location)
    {
        return new LogEvent("open_location_chest", "location_id", location);
    }

    public void SendLog()
    {
        FirebaseAnalytics.LogEvent(firebaseName, paramName, value);
    }

}

