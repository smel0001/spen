using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    //global map
    public Tilemap map;
    public Sprite TEMPTILE;
    Chunk aChunk;

    void Awake()
    {

    }

    void Start()
    {
        aChunk = new Chunk();
        DrawChunk(0);
    }

    void DrawChunk(int offset)
    {
        for (int i = 0; i < Chunk.size; i++)
        {
            for (int j = 0; j < Chunk.size; j++)
            {
                map.SetTile(new Vector3Int(i + offset, j + offset, 0), aChunk.GetTile(i, j));
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
