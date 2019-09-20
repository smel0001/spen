using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public static int size = 16;
    Tile[,] tiles;

    public Sprite full;
    public Sprite empty;

    void Awake()
    {
        tiles = new Tile[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                tiles[i, j] = new Tile(i + (int)transform.position.x, j + (int)transform.position.y);

                //jank tile spawn
                GameObject tileGO = new GameObject("Tile_ " + i + (int)transform.position.x + j + (int)transform.position.y);
                tileGO.transform.position = new Vector3(tiles[i, j].x, tiles[i, j].y, 0f);
                tileGO.transform.SetParent(this.transform, true);
                SpriteRenderer spriteRenderer = tileGO.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = full;
            }
        }
    }

    void Update()
    {
        
    }
}
