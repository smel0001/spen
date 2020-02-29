using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class ItemDatabase
{
    #region Singleton
    private static readonly ItemDatabase t_Instance = new ItemDatabase();

    static ItemDatabase()
    {
        Instance.LoadItemDatabase();
    }
    //private BiomeData(){}

    public static ItemDatabase Instance
    {
        get {return t_Instance;}
    }
    #endregion

    private List<Item> database = new List<Item>();
    private static string itemDatabaseFileName = "/StreamingAssets/Items.json";
    private static string spawnerFilePath = "/StreamingAssets/SpawnerItems.json";
    private static string tileSpawnerFilePath = "/StreamingAssets/TileSpawnerItems.json";

    void LoadItemDatabase()
    {
        string filePath = Application.dataPath + itemDatabaseFileName;
        Item[] items = JsonHelper.FromJson<Item>(File.ReadAllText(filePath));
        for (int i = 0; i < items.Length; i++)
        {
            //TODO move item init to inside the FromJson call
            items[i].Init();
            database.Add(items[i]);
        }

        //Item, EquipItem, UsableItem, Consumable, Interactor, Spawner, TileSpawner 
        filePath = Application.dataPath + spawnerFilePath;
        SpawnerItem[] spawnerItems = JsonHelper.FromJson<SpawnerItem>(File.ReadAllText(filePath));
        for (int i = 0; i < spawnerItems.Length; i++)
        {
            //TODO move item init to inside the FromJson call
            spawnerItems[i].Init();
            database.Add(spawnerItems[i]);
        }

        filePath = Application.dataPath + tileSpawnerFilePath;
        TileSpawnerItem[] tileSpawnerItems = JsonHelper.FromJson<TileSpawnerItem>(File.ReadAllText(filePath));
        for (int i = 0; i < tileSpawnerItems.Length; i++)
        {
            //TODO move item init to inside the FromJson call
            tileSpawnerItems[i].Init();
            database.Add(tileSpawnerItems[i]);
        }
    }

    //Debug/Dev Function
    void SaveItemDatabase()
    {
        Item[] items = database.ToArray();
        string myjson = JsonHelper.ToJson(items, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/Items.json", myjson);
    }

    public Item FetchItemById(int id)
    {
        return database.Find(x => x.ID == id);
    }
}

//From:
//Functions for JsonUtility to manage arrays
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
 }