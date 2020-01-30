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
        RuleTile ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/DirtToLGrass");
        switch (tiles[x, y].tileBiome)
        {
            case BiomeType.Forest:
                ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/DirtToLGrass");
                break;
            case BiomeType.Ocean:
                ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/WaterToGrass");
                break;
            case BiomeType.Desert:
                ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/WaterToGrass");
                break;
            case BiomeType.Mountain:
                ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/DirtToLGrass");
                break;
            case BiomeType.Edge:
                break;
        }

        return ruleTile;
    }

    public void Tick()
    {
        
    }
}
