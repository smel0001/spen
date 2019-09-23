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

    public Chunk(int _x, int _y, BiomeType _biome)
    {
        ChunkBiome = _biome;
        coords = new Vector2Int(_x, _y);
        tiles = new Tile[size, size];

        Sprite _sprite = SpriteManager.Instance.GetSprite("Grass");
        switch (ChunkBiome)
        {
            case BiomeType.Forest:
                //set generator settings
                _sprite = SpriteManager.Instance.GetSprite("Grass");
                break;
            case BiomeType.Ocean:
                _sprite = SpriteManager.Instance.GetSprite("Water");
                break;
            case BiomeType.Edge:
                break;
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                tiles[i, j] = ScriptableObject.CreateInstance<Tile>();
                tiles[i, j].sprite = _sprite;
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }
}
