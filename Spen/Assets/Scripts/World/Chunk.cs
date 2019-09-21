using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public static int size = 16;
    Tile[,] tiles;

    public Chunk()
    {
        tiles = new Tile[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                tiles[i, j] = ScriptableObject.CreateInstance<Tile>();
                tiles[i, j].sprite = SpriteManager.Instance.GetSprite("Grass");
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }
}
