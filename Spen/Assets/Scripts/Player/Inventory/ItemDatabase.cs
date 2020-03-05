using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public sealed class ItemDatabase
{
    #region Singleton
    private static readonly ItemDatabase t_Instance = new ItemDatabase();

    static ItemDatabase()
    {
        Instance.LoadItemDatabase();
    }

    public static ItemDatabase Instance
    {
        get {return t_Instance;}
    }
    #endregion

    private List<Item> database = new List<Item>();
    private static string itemDatabaseFileName = "/StreamingAssets/MasterItems.json";

    void LoadItemDatabase()
    {

        JsonSerializerSettings setting = new JsonSerializerSettings();
        setting.DefaultValueHandling = DefaultValueHandling.Populate;

        string filePath = Application.dataPath + itemDatabaseFileName;

        string text = File.ReadAllText(filePath);
        string[] buckets = text.Split('#');

        foreach (string itemTypeData in buckets)
        {
            int newlineIndex = itemTypeData.IndexOf(System.Environment.NewLine);
            string typeText = itemTypeData.Substring(0, newlineIndex);
            string jsonText = itemTypeData.Substring(newlineIndex + 1);

            switch(typeText)
            {
                case "Item":
                    List<Item> itemList = JsonConvert.DeserializeObject<List<Item>>(jsonText, setting);
                    AddItemListToDB(itemList);
                    break;
                case "EquipItem":
                    List<EquipItem> equipItemList = JsonConvert.DeserializeObject<List<EquipItem>>(jsonText, setting);
                    AddItemListToDB(equipItemList);
                    break;
                case "UsableItem":
                    List<UsableItem> usableItemList = JsonConvert.DeserializeObject<List<UsableItem>>(jsonText, setting);
                    AddItemListToDB(usableItemList);
                    break;
                case "SpawnerItem":
                    List<SpawnerItem> spawnItemList = JsonConvert.DeserializeObject<List<SpawnerItem>>(jsonText, setting);
                    AddItemListToDB(spawnItemList);
                    break;
                case "TileSpawnerItem":
                    List<TileSpawnerItem> tileSpawnItemList = JsonConvert.DeserializeObject<List<TileSpawnerItem>>(jsonText, setting);
                    AddItemListToDB(tileSpawnItemList);
                    break;
            }
        }
    }

    void AddItemListToDB<T>(List<T> list) where T : Item
    {
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Init();
                database.Add(list[i]);
            }
        }
    }
    
    public Item FetchItemById(int id)
    {
        return database.Find(x => x.ID == id);
    }
}