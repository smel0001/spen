using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public static int size = 16;
    public Vector2Int coords;
    Tile[,] tiles;
    //Biome?
    public BiomeType ChunkBiome;

    public RuleTile ruleTile;

    public Chunk(int _x, int _y, GenTile[,] map)
    {
        coords = new Vector2Int(_x, _y);
        tiles = new Tile[size, size];

        Sprite _sprite = SpriteManager.Instance.GetSprite("Grass");
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                switch (map[i, j].tileBiome)
                {
                    case BiomeType.Forest:
                        _sprite = SpriteManager.Instance.GetSprite("Grass");
                        ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/DirtToLGrass");
                        break;
                    case BiomeType.Ocean:
                        _sprite = SpriteManager.Instance.GetSprite("Water");
                        ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/WaterToGrass");
                        break;
                    case BiomeType.Desert:
                        _sprite = SpriteManager.Instance.GetSprite("Sand");
                        ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/WaterToGrass");
                        break;
                    case BiomeType.Mountain:
                        _sprite = SpriteManager.Instance.GetSprite("Rock");
                        ruleTile = Resources.Load<RuleTile>("Sprites/RuleTiles/DirtToLGrass");
                        break;
                    case BiomeType.Edge:
                        break;
                }

                tiles[i, j] = ScriptableObject.CreateInstance<Tile>();
                tiles[i, j].sprite = _sprite;
            }
        }

    }

    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public void Tick()
    {
        
    }
}
