using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class DataSaver
{
    private const string file_name = "/Save2.dat";
    private const string version = "1.2.0";

    private static Dictionary<string, object> data;
    private static List<IAutoSaving> autoSavingObjects = new List<IAutoSaving>();

    public static bool IsDataLoaded { private set; get; }

    public static int UniqueRandomSeed => (int)data["unique_random_seed"];

    static DataSaver()
    {
        ReadDataFromDrive();
        if (data == null) {
            data = new Dictionary<string, object>() {
                { "version", version },
                { "unique_random_seed", DateTime.Now.Millisecond }
            };
        }
    }

    public static void SetKey(string keyName, object obj)
    {
        if (!HasKey(keyName))
            data.Add(keyName, obj);
        else data[keyName] = obj;
    }

    public static object GetKey(string key)
    {
        return data[key];
    }

    public static bool HasKey(string key)
    {
        return data.ContainsKey(key);
    }

    public static void AddAutoSaveObject(IAutoSaving obj)
    {
        autoSavingObjects.Add(obj);
    }

    public static void RemoveAutoSaveObject(IAutoSaving obj)
    {
        autoSavingObjects.Remove(obj);
    }

    public static void AutoSave()
    {
        foreach (IAutoSaving auto in autoSavingObjects)
            data[auto.KeyName] = auto.GetSavingData();
    }

    public static void SaveData()
    {
        AutoSave();
        WriteDataOnDrive();
    }

    public static void WriteDataOnDrive()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + file_name);
        bf.Serialize(file, data);
        file.Close();
    }

    public static void ReadDataFromDrive()
    {
        if (File.Exists(Application.persistentDataPath + file_name)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + file_name, FileMode.Open);
            file.Position = 0;
            data = (Dictionary<string, object>)bf.Deserialize(file);
            file.Close();
            IsDataLoaded = true;
        }
    }

}

public interface IAutoSaving
{
    public string KeyName { get; }

    public object GetSavingData();
}