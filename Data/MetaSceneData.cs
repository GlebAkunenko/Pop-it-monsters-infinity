using System;
using UnityEngine;
using System.Security.Cryptography;
using Firebase.Analytics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public static class MetaSceneData
{
    public const string buttle_level_name = "Level";
    public const string infinity_level_name = "InfinityLevel";
    public const string chest_scene_name = "ChestOpening";
    public const string review_scene_name = "Level Rating";
    public const string first_level_scene_name = "Forest";
    public const string shop_scene_name = "Shop";

    public static InfinityLevelData InfinityLevelData { get; set; }
    public static LocationChallengesData ChallengesData { get; set; }
    public static Challange Challange { get; set; }
    public static bool TrainingMode { get; set; }
    public static IChestInfo ChestLoot { get; set; }
    public static PageCollection ChestLootCollection { get; set; }
    public static string ChestOpeningBackSceneName { get; set; }

    private static Savable<bool> unlockNewMonster;

    static MetaSceneData()
    {
        unlockNewMonster = new Savable<bool>(false, "new_monster_unlocked");
    }

    public static bool UnlockNewMonster
    {
        get => unlockNewMonster;
        set => unlockNewMonster.Set(value);
    }



    #region Use only in classic (old) version

    public static OptionsData OptionsData { get; set; } = new OptionsData();
    public static PlayerStats Player { get; set; } = PlayerStats.Empty;
    public static bool OptionalLevel { get; set; }
    public static int Level_id { get; set; }
    public static bool Win { get; set; }
    public static LevelData LevelData { get; set; }
    public static bool Started { get; set; }
    public static bool InShop { get; set; }
    public static GameData GameData { get; set; } = new GameData();
    public static LevelStatistics Statistics { get; set; } = new LevelStatistics();



    private static List<Guide> showedGuides = new List<Guide>();
    
    public static bool IsGuideShown(Guide guide)
    {
        return showedGuides.Contains(guide);
    }

    public static void ShowGuide(Guide guide)
    {
        if (!showedGuides.Contains(guide))
            showedGuides.Add(guide);
    }

    public static bool EnableMounseDown { get; set; }

    public static string AnaliticsLevelName
    {
        get { return GameData.LocationName + "_" + Level_id.ToString(); }
    }

    [Serializable]
    private class SerializableObject
    {
        public PlayerStats Player { get; set; } = PlayerStats.Empty;
        public GameData GameData { get; set; }
        public OptionsData OptionsData { get; set; }
        public List<Guide> ShowedGuides { get; set; }
    }

    /// <summary>
    /// Use only in classic mode. After infinity update use DataSaver
    /// </summary>
    public static void SaveData()
    {
        foreach (Location loc in GameData.Locations)
            loc.SetSerializeVectoresBeforeSerializing();

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/Save.dat");

        SerializableObject data = new SerializableObject() {
            Player = Player,
            GameData = GameData,
            OptionsData = OptionsData,
            ShowedGuides = showedGuides
        };

        bf.Serialize(file, data);

        file.Close();
        Debug.Log("Game data saved! " + Application.persistentDataPath);
    }

    /// <summary>
    /// Use only in classic mode. After infinity update use DataSaver
    /// </summary>
    public static void LoadData(string[] locationsName)
    {
        if (File.Exists(Application.persistentDataPath + "/Save.dat")) {

            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/Save.dat", FileMode.Open);
            file.Position = 0;
            SerializableObject data = (SerializableObject)bf.Deserialize(file);
            file.Close();

            Player = data.Player;
            GameData = data.GameData;
            OptionsData = data.OptionsData;

            // after 1.0.2
            showedGuides = data.ShowedGuides;
            if (showedGuides == null)
                showedGuides = new List<Guide>();

            foreach (Location loc in GameData.Locations)
                loc.SetVectoresAfterDeserializing();

            List<Location> locationsList = new List<Location>();
            foreach(string name in locationsName) {
                Location inited = LocationInited(name, GameData.Locations);
                if (inited == null)
                    inited = new Location(name);
                locationsList.Add(inited);
            }

            GameData.Locations = locationsList.ToArray();

            foreach (Location location in MetaSceneData.GameData.Locations)
                location.UpdateAllStars();

            Debug.Log("Game data loaded!");
        }
        else {
            Player = PlayerStats.Empty;
            GameData = new GameData();
            Location[] locations = CreateLocations(locationsName);
            GameData.Locations = locations;
            GameData.LocationName = locations[0].Name;
        }

        Location.LocationNames = locationsName;
    }

    private static Location[] CreateLocations(string[] locationsName)
    {
        Location[] locations = new Location[locationsName.Length];
        for (int i = 0; i < locationsName.Length; i++)
            locations[i] = new Location(locationsName[i]);
        return locations;
    }

    public static Location LocationInited(string name, Location[] locations)
    {
        foreach(Location location in locations) {
            if (location.Name == name)
                return location;
        }
        return null;
    }

    public static bool CheckFirstOpen()
    {
        return File.Exists(Application.persistentDataPath + "/Save.dat");
    }

    #endregion

}

