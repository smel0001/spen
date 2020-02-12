/*
Singleton class to load and store biome data.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class BiomeData
{
    #region Singleton
    private static readonly BiomeData t_Instance = new BiomeData();

    static BiomeData()
    {
        Debug.Log("static runs");
        Instance.LoadAll();
    }
    //private BiomeData(){}

    public static BiomeData Instance
    {
        get {return t_Instance;}
    }
    #endregion

    //Height types list
    private List<Terrain> terrain = new List<Terrain>();

    private List<BiomeCategory> temperatureCategories = new List<BiomeCategory>();
    private List<BiomeCategory> moistureCategories = new List<BiomeCategory>();

    /*
     * iterate over tempCategories to find 1st digit
     * iterate over moistureCategories to find 2nd digit
     * 
     * This is what keys in biome dict will look like
     * 00 01 02 03
     * 10 11 12 13
     * 20 21 22 23
     * 30 31 32 33
     * 
     * coldest  cold    hot     hottest
     * tundra   tundra  grass   desert
     * tundra   boreal  savanna savanna
     * tundra   boreal  boreal  tropical
     * tundra   tundra  tropical    tropical
     * 
     */
    private Dictionary<string, Biome> biomeDict = new Dictionary<string, Biome>();

    private static string terrainDataFileName = "/StreamingAssets/Terrain.json";
    private static string biomeCategoryDataFileName = "/StreamingAssets/BiomeCategories.json";
    private static string biomeDataFileName = "/StreamingAssets/Biomes.json";


    public void LoadAll()
    {
        LoadTerrainData();
        LoadBiomeCategoryData();
        LoadBiomeData();
    }

    void LoadTerrainData()
    {
        string filePath = Application.dataPath + terrainDataFileName;
        Terrain[] terrainArray = JsonHelper.FromJson<Terrain>(File.ReadAllText(filePath));
        for (int i = 0; i < terrainArray.Length; i++)
        {
            terrain.Add(terrainArray[i]);
        }
    }

    void LoadBiomeCategoryData()
    {
        string filePath = Application.dataPath + biomeCategoryDataFileName;
        BiomeCategory[] biomesArray = JsonHelper.FromJson<BiomeCategory>(File.ReadAllText(filePath));
        for (int i = 0; i < biomesArray.Length; i++)
        {
            if (biomesArray[i].isTemperature)
            {
                temperatureCategories.Add(biomesArray[i]);
            }
            else
            {
                moistureCategories.Add(biomesArray[i]);
            }
        }
        temperatureCategories.Sort();
        moistureCategories.Sort();
    }

    void LoadBiomeData()
    {
        string filePath = Application.dataPath + biomeDataFileName;
        Biome[] biomesArray = JsonHelper.FromJson<Biome>(File.ReadAllText(filePath));
        for (int i = 0; i < biomesArray.Length; i++)
        {
            biomesArray[i].Init();
            foreach (string key in biomesArray[i].Keys)
            {
                biomeDict.Add(key, biomesArray[i]);
            }
        }
    }


    public Terrain GetTerrain(float height)
    {
        for (int i = 0; i < terrain.Count; i++)
        {
            if (height <= terrain[i].HeightThreshold)
            {
                return terrain[i];
            }
        }
        return null;
    }

    public Biome GetBiome(float height, float temperature, float moisture)
    {
        Terrain tileTerrain = terrain[0];
        for (int i = 0; i < terrain.Count; i++)
        {
            if (height <= terrain[i].HeightThreshold)
            {
                tileTerrain = terrain[i];
                break;
            }
        }

        //if terrain is water
        if (tileTerrain.ID == 0)
        {
            //return ocean
            return biomeDict["99"];
        }

        string key = "";

        for (int i = 0; i < moistureCategories.Count; i++)
        {
            if (moisture <= moistureCategories[i].Threshold)
            {
                key += i.ToString();
                break;
            }
        }

        for (int i = 0; i < temperatureCategories.Count; i++)
        {
            if (temperature <= temperatureCategories[i].Threshold)
            {
                key += i.ToString();
                break;
            }
        }

        return biomeDict[key];
    }
}

[System.Serializable]
public class Terrain
{
    public int ID;
    public string Title;
    public float HeightThreshold;
}

[System.Serializable]
public class BiomeCategory : System.IComparable
{
    public int ID;
    public string Title;
    public bool isTemperature;
    public float Threshold;

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;
        else
            return this.Threshold.CompareTo((obj as BiomeCategory).Threshold);
    }
}

[System.Serializable]
public class Biome
{
    public int ID;
    public string Title;
    public string TileSlug;
    public List<string> Keys;
    public bool SpawnTrees;

    [System.NonSerialized]
    public RuleTile BiomeTile;
    [System.NonSerialized]
    public GameObject Treefab;

    public void Init()
    {
        string slug = "Sprites/RuleTiles/" + this.TileSlug;
        BiomeTile = Resources.Load<RuleTile>(slug);

        slug = "Prefabs/Trees/" + this.TileSlug;
        Treefab = Resources.Load<GameObject>(slug);
    }
}