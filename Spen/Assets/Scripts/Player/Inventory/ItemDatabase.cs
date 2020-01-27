using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();

    void Start()
    {
        LoadItemDatabase();
    }

    void LoadItemDatabase()
    {
        Item[] items = JsonHelper.FromJson<Item>(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        for (int i = 0; i < items.Length; i++)
        {
            //TODO move item init to inside the FromJson call
            items[i].Init();
            database.Add(items[i]);
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