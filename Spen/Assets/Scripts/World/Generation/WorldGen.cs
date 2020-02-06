using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct GenTile
{
    public Biome tileBiome;
    public int x, y;

    public GenTile(int _x, int _y, Biome _biome)
    {
        x = _x;
        y = _y;
        tileBiome = _biome;
    }
}

public class WorldGen : MonoBehaviour
{
    //global map
    public Tilemap map;

    public float levelScale;

    [SerializeField]
    private AnimationCurve heightCurve;
    public NoiseMap.Wave[] heightWaves;

    [SerializeField]
    private AnimationCurve temperatureCurve;
    public NoiseMap.Wave[] temperatureWaves;

    [SerializeField]
    private AnimationCurve moistureCurve;
    public NoiseMap.Wave[] moistureWaves;

    public static int WorldSize = 32;
    private int numChunks = WorldSize / Chunk.size;

    private Dictionary<System.Tuple<int, int>, GenTile[,]> fullchunkmap;
    private Dictionary<System.Tuple<int,int>,Chunk> loadedChunks;

    ///TEMP
    GameObject player;
    System.Tuple<int, int> curChunk;
    int curx = 0;
    int cury = 0;

    void Awake()
    {
        fullchunkmap = new Dictionary<System.Tuple<int,int>, GenTile[,]>();
        loadedChunks = new Dictionary<System.Tuple<int, int>, Chunk>();
    }

    void Start()
    {
        //Biome Generation
        //maybe use a features/structures.json (or just classes) that can be placed in a final pass
           
        //Pass 1: HEIGHT
        float[,] height = NoiseMap.GeneratePerlinNoiseMap(WorldSize, WorldSize, levelScale, 0, 0, heightWaves);

        //Pass 2: HEAT
        float[,] temperature = NoiseMap.GenerateUniformNoiseMap(WorldSize, WorldSize, WorldSize / 2, WorldSize/2, 0);
        float[,] randomTemperature = NoiseMap.GeneratePerlinNoiseMap(WorldSize, WorldSize, levelScale, WorldSize, 0, temperatureWaves);
        float[,] heatMap = new float[WorldSize, WorldSize];

        for (int yIndex = 0; yIndex < WorldSize; yIndex++)
        {
            for (int xIndex = 0; xIndex < WorldSize; xIndex++)
            {
                heatMap[xIndex, yIndex] = temperature[xIndex, yIndex] * randomTemperature[xIndex, yIndex];
                heatMap[xIndex, yIndex] += temperatureCurve.Evaluate(height[xIndex, yIndex]) * heatMap[xIndex, yIndex];

                heatMap[xIndex, yIndex] = Mathf.Clamp(heatMap[xIndex, yIndex], 0f, 1f);
            }
        }

        //Pass 3: MOISTURE
        float[,] moistureMap = NoiseMap.GeneratePerlinNoiseMap(WorldSize, WorldSize, levelScale, 0, 0, moistureWaves);
        for (int yIndex = 0; yIndex < WorldSize; yIndex++)
        {
            for (int xIndex = 0; xIndex < WorldSize; xIndex++)
            {
                moistureMap[xIndex, yIndex] -= this.moistureCurve.Evaluate(height[xIndex, yIndex]) * moistureMap[xIndex, yIndex];

                moistureMap[xIndex, yIndex] = Mathf.Clamp(moistureMap[xIndex, yIndex], 0f, 1f);
            }
        }

        //BASE TILES
        GenTile[,] generatedtiles = new GenTile[WorldSize, WorldSize];
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                Biome chosenBio = BiomeData.Instance.GetBiome(height[i, j], heatMap[i, j], moistureMap[i, j]);
                generatedtiles[i, j] = new GenTile(i, j, chosenBio);
            }
        }


        //Generate
        //List<GenTile[,]> chunkMapList = new List<GenTile[,]>();
        for (int i = 0; i < numChunks; i++)
        {
            for (int j = 0; j < numChunks; j++)
            {
                //gen chunk
                GenTile[,] chunkMap = new GenTile[Chunk.size, Chunk.size];
                for (int k = 0; k < Chunk.size; k++)
                {
                    for (int z = 0; z < Chunk.size; z++)
                    {
                        chunkMap[k, z] = generatedtiles[k + i*Chunk.size, z + j*Chunk.size];
                    }
                }

                fullchunkmap.Add(new System.Tuple<int, int>(i, j), chunkMap);
            }
        }

        //Save all chunks to disk? i.e. out of ram?
        //Load current nearby chunks


        ///TEMP
        player = GameObject.FindWithTag("Player");
        LoadAllChunks();
    }

    //need to rename
    void GenerateBlock()
    { }

    void LoadChunk(int x, int y)
    {
        GenTile[,] chunkmap = fullchunkmap[new System.Tuple<int, int>(x, y)];
        Chunk chunk = new Chunk(x * Chunk.size, y * Chunk.size, chunkmap);
        loadedChunks.Add(new System.Tuple<int, int>(chunk.coords.x / Chunk.size, chunk.coords.y / Chunk.size), chunk);

        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(chunk.coords.x + i, chunk.coords.y + j, 0), chunk.GetTile(i,j));
            }
        }
    }

    void UnloadChunk(int x, int y)
    {
        System.Tuple<int, int> chunktuple = new System.Tuple<int, int>(x, y);
        Chunk chunk = loadedChunks[chunktuple];

        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(chunk.coords.x + i, chunk.coords.y + j, 0), null);
            }
        }
        loadedChunks.Remove(chunktuple);
    }

    /* 
     * 3. Chunk
     * 4. Tiles
     * 5.       Chunk loading  <----
     * 6. Biome logic behind chunks 
     * 
     * Chunks are either:
     * an actual tilemap (i.e. several tilemaps)
     * a sub data structure of tilemap (collection of tiles that fill the tilemap) <- leaning towards this
     */


    void LoadAllChunks()
    {
        for (int i = 0; i < numChunks; i++)
        {
            for (int j= 0; j < numChunks; j++)
            {
                LoadChunk(i, j);
            }
        }
    }


    void LoadPlayerChunk()
    {
        int playerX = (int)player.transform.position.x / Chunk.size;
        int playerY = (int)player.transform.position.y / Chunk.size;

        if (curx != playerX || cury != playerY)
        {
            UnloadChunk(curx, cury);
            curx = playerX;
            cury = playerY;
            LoadChunk(playerX, playerY);
        }
    }
}