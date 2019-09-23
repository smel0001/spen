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
    Chunk aChunk;
    Chunk anotherChunk;

    //Temp?
    public static int WorldSize = 64;
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
                if (i > WorldSize / 2 && j > WorldSize / 2)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Forest);
                }
                else if (i <= WorldSize / 2 && j <= WorldSize / 2)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Ocean);
                }
                else if (i > WorldSize / 2 && j <= WorldSize / 2)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Forest);
                }
                else if (i <= WorldSize / 2 && j > WorldSize / 2)
                {
                    generatedtiles[i, j] = new GenTile(i, j, BiomeType.Forest);
                }
            }
        }
        //generatedtiles is the biome map this
        int xoffset = 24;
        int yoffset = 0;
        GenTile[,] chunk = new GenTile[Chunk.size, Chunk.size];
        //Manual array copy?
        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                chunk[i, j] = generatedtiles[i + xoffset, j + yoffset];
            }
        }



        int rowLength = chunk.GetLength(0);
        int colLength = chunk.GetLength(1);
        string arrayString = "";
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                arrayString += string.Format("{0} ", chunk[i, j].tileBiome);
            }
            arrayString += System.Environment.NewLine + System.Environment.NewLine;
        }
        Debug.Log(arrayString);

        //PRINT ARRAY TO CONSOLE DEBUG

        //Chunk generation takes subset of this big array and creates tiles

        //Using Biome map, we load/unload required chunks
        //Chunk
        // - Creates actual tiles when chunk is loaded
        // - Removes actual tiles on unload


        //Debug.Log("" + Chunk.size * -1);
       // aChunk = new Chunk(Chunk.size * -1, Chunk.size * 0, BiomeType.Forest);
       // anotherChunk = new Chunk(Chunk.size * 0, Chunk.size * 0, BiomeType.Ocean);
       // DrawChunk();
    }

    void DrawChunk()
    {
        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(aChunk.coords.x + i, aChunk.coords.y + j, 0), aChunk.GetTile(i, j));
                map.SetTile(new Vector3Int(anotherChunk.coords.x + i, anotherChunk.coords.y + j, 0), anotherChunk.GetTile(i, j));
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
