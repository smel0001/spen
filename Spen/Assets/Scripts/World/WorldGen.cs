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

    //Temp?
    public static int WorldSize = 64;
    private int numChunks = WorldSize / Chunk.size;
    private int numBiomes = 4;

    private Dictionary<SystemTuple<int, int>, GenTile[,]> fullchunkmap;

    private List<Chunk> loadedChunks;
    private Dictionary<System.Tuple<int,int>,Chunk> michunks;

    ///TEMP
    GameObject player;
    System.Tuple<int, int> curChunk;

    void Awake()
    {
        fullchunkmap = new Dictionary<System.Tuple<int,int>, GenTile[,]>();
        michunks = new Dictionary<System.Tuple<int, int>, Chunk>();
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
                float gen = Mathf.PerlinNoise(i * 0.002f , j * 0.002f);
                Debug.Log("" + gen);

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


        //Sort filled world into chunks to save + load

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
                michunks.Add(new System.Tuple<int, int>(i, j), new Chunk(i * Chunk.size - 32, j * Chunk.size - 32);

                //Temp draw all chunk
                DrawChunk(new Chunk(i * Chunk.size - 32, j * Chunk.size - 32, chunkMap));
            }
        }

        //Save all chunks to disk? i.e. out of ram?
        //Load current nearby chunks


        ///TEMP
        player = GameObject.FindWithTag("Player");
        curChunk = new System.Tuple<int, int>(0,0);
    }

    void DrawChunk(Chunk chunk)
    {
        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(chunk.coords.x + i, chunk.coords.y + j, 0), chunk.GetTile(i, j));
            }
        }
        loadedChunks.Add(chunk);
    }

    void UnloadChunk(Chunk chunk)
    {
        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(chunk.coords.x + i, chunk.coords.y + j, 0), null);
            }
        }
        loadedChunks.Remove(chunk);
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



     void Update()
     {
         //Update loaded chunks
         foreach (Chunk chnk in loadedChunks)
         {
             chnk.Tick();
         }

         //temp only load chunk the player is in
        Debug.Log("" + player.transform.position);
        int playerX = int(player.transform.position.x);
        int playerY = int(player.transform.position.y);

        //when player leaves chunk load and unload
        curChunk

     }
}
