/*
Data structure to hold tile information. To be expanded to allow live loading/unloading of chunks
as well as storing object data.
*/

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

    public Chunk(int _x, int _y, GenTile[,] map)
    {
        coords = new Vector2Int(_x, _y);
        tiles = map;
    }

    public RuleTile GetTile(int x, int y)
    {
        return tiles[x, y].tileBiome.BiomeTile;
    }

    public void Tick()
    {
        
    }
}
