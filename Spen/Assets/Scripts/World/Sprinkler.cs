using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sprinkler : MonoBehaviour
{
    Tilemap map;
    private Vector3Int CellPos;
    private List<SoilTile> AdjacentSoil = new List<SoilTile>();
    private float timer = 0f;
    private float WaterTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        //Needs its tilemap somehow idk
        map = GameObject.Find("Clickable").GetComponent<Tilemap>();
        CellPos = map.WorldToCell(transform.position);
        UpdateAdjacentSoil();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = WaterTime;
            WaterSoil();
        }
    }

    void WaterSoil()
    {
        foreach (SoilTile soil in AdjacentSoil)
        {
            soil.Saturation += 10f;
        }
    }

    void UpdateAdjacentSoil()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i != 0 || j != 0)
                {
                    TileBase tile = map.GetTile(new Vector3Int(CellPos.x + i, CellPos.y + j, 0));
                    SoilTile soilTile = tile as SoilTile;
                    if (soilTile)
                    {
                        AdjacentSoil.Add(soilTile);
                    }
                }
            }
        }
        Debug.Log(AdjacentSoil.Count);
    }
}
