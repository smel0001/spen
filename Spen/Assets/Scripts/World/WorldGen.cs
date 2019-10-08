using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct GenTile
{
    public BiomeType tileBiome;
    public int x, y;

    public GenTile(int _x, int _y, BiomeType _biome)
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

    public static int WorldSize = 16;
    private int numChunks = WorldSize / Chunk.size;

    //not used yet
    private int numBiomes = 4;

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
        // - Biome just creates map, doesn't load real tiles

        //Insert Generation algorithms here
        //Fill world with tiles (this is where we calculate biome)
        GenTile[,] generatedtiles = new GenTile[WorldSize, WorldSize];
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                float gen = Mathf.PerlinNoise(i * 0.005f , j * 0.005f);

                if (gen < 0.25f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Ocean);
                }
                else if (gen >= 0.25f && gen < 0.4f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Desert);
                }
                else if (gen >= 0.4f && gen < 0.75f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Forest);
                }
                else if (gen >= 0.75f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Mountain);
                }
            }
        }

        //Generate
        List<GenTile[,]> chunkMapList = new List<GenTile[,]>();

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

    void LoadChunk(int x, int y)
    {
        //Load chunk from map
        GenTile[,] chunkmap = fullchunkmap[new System.Tuple<int, int>(x, y)];
        Chunk chunk = new Chunk(x * Chunk.size, y * Chunk.size, chunkmap);
        loadedChunks.Add(new System.Tuple<int, int>(chunk.coords.x / Chunk.size, chunk.coords.y / Chunk.size), chunk);

        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(chunk.coords.x + i, chunk.coords.y + j, 0), chunk.GetTile(i, j));
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

    void Update()
     {
         //Update loaded chunks
         //foreach (Chunk chnk in loadedChunks)
         //{
         //    chnk.Tick();
         //}
     }
}
