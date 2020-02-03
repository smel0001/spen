using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private List<Item> database = new List<Item>();
    private static string itemDatabaseFileName = "/StreamingAssets/Items.json";
    private static string placeItemDatabaseFileName = "/StreamingAssets/PlaceItems.json";


    void Start()
    {
        LoadItemDatabase();
    }

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

        string placeFilePath = Application.dataPath + placeItemDatabaseFileName;
        PlaceItem[] placeItems = JsonHelper.FromJson<PlaceItem>(File.ReadAllText(placeFilePath));
        for (int i = 0; i < placeItems.Length; i++)
        {
            //TODO move item init to inside the FromJson call
            placeItems[i].Init();
            database.Add(placeItems[i]);
        }

        Debug.Log(database[database.Count - 1].Title);



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