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

    //Loaded Chunks Container

    void Awake()
    {

    }

    void Start()
    {
        //Biome Generation
        // - Biome just creates map, doesn't load real tiles

        //Temp simple generate
        GenTile[,] generatedtiles = new GenTile[WorldSize, WorldSize];
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                float gen = Mathf.PerlinNoise(i * 0.15f , j * 0.15f);
                Debug.Log("" + gen);

                if (gen < 0.25f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Ocean);
                }
                else if (gen >= 0.25f && gen < 0.5f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Desert);
                }
                else if (gen >= 0.5f && gen < 0.75f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Forest);
                }
                else if (gen >= 0.75f)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Mountain);
                }

                /*
                switch (Random.Range(1, 5))
                {
                    case 1:
                        generatedtiles[i, j] = new GenTile(i, j, BiomeType.Ocean);
                        break;
                    case 2:
                        generatedtiles[i, j] = new GenTile(i, j, BiomeType.Forest);
                        break;
                    case 3:
                        generatedtiles[i, j] = new GenTile(i, j, BiomeType.Desert);
                        break;
                    case 4:
                        generatedtiles[i, j] = new GenTile(i, j, BiomeType.Mountain);
                        break;
                }
                */
            }
        }

        //Generate 
        List<GenTile[,]> chunkMapList = new List<GenTile[,]>();
        //gen all chunks
        
        Debug.Log("numchunk: " + numChunks);
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
                chunkMapList.Add(chunkMap);
                DrawChunk(new Chunk(i * Chunk.size - 32, j * Chunk.size - 32, chunkMap));
            }
        }


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
}
