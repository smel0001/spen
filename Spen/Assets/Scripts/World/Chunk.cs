using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public static int size = 16;
    public Vector2Int coords;
    GenTile[,] tiles;
    //Biome?
    public BiomeType ChunkBiome;

    public Chunk(int _x, int _y, GenTile[,] map)
    {
        coords = new Vector2Int(_x, _y);
        tiles = map;
    }

    public RuleTile GetTile(int x, int y)
    {
        string slug = "Sprites/RuleTiles/" + tiles[x, y].tileBiome.TileSlug;
        RuleTile ruleTile = Resources.Load<RuleTile>(slug);

        return ruleTile;
    }

    public void Tick()
    {
        
    }
}
