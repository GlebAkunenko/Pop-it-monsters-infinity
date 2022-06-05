using System;

[System.Serializable]
public class LocationChallengesData
{
    private InfinityLevelData infinityLevelData;
    private PageCollection collection;
    private PlayerStorageData storageData;

    public bool IsInited { private set; get; }
    public int LocationId { private set; get; }
    public InfinityLevelData InfinityLevelData  => infinityLevelData;
    public PageCollection Collection  => collection;
    public PlayerStorageData StorageData => storageData;

    public void Init(InfinityLevelData levelData)
    {
        if (!levelData.IsInited)
            throw new Exception("Level data is not inited");

        infinityLevelData = levelData;
        collection = levelData.MonstersCollection;
        LocationId = levelData.Id;
        storageData = PlayerStorageData.GetInstance();

        IsInited = true;
    }

}
