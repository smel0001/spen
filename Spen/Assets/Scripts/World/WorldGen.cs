using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    //global grid
    Grid globalGrid;
    Tilemap map;
    void Awake()
    {
        map = new Tilemap();
        globalGrid = new Grid();
    }
    /*
     * 1. World Grid
     * (not sure how these two work together)
     * 2. Create a Tilemap https://medium.com/@allencoded/unity-tilemaps-and-storing-individual-tile-data-8b95d87e9f32
     * 3. Chunk
     * 4. Tiles
     * 5. Chunk loading
     * 6. Biome logic behind chunks 
     * 
     * Chunks are either:
     * an actual tilemap (i.e. several tilemaps)
     * a sub data structure of tilemap (collection of tiles that fill the tilemap) <- leaning towards this
     */
}
