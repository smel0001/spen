using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class SpawnerItem : InteractorItem
{
    [SerializeField]
    private string PrefabSlug;
    [SerializeField]
    private bool SpawnAtCursor;
    [SerializeField]
    private bool SpawnInGrid;

    private GameObject prefab;  //TEMP, find better way to do this, maybe as static etc
    
    public SpawnerItem(int id, string title, int value, string slug) : base(id, title, value, slug)
    {}

    protected override void ExtendInit()
    {
        this.prefab = Resources.Load<GameObject>("Prefabs/" + PrefabSlug);
        cursor = GameObject.Find("UI/Canvas/Cursor").GetComponent<Cursor>();
    }

    protected override void Interact<T>(T obj)
    {
        Vector3 pos = Vector3.zero;
        if (SpawnAtCursor)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
        }
        else
        {
            pos = GameObject.Find("Player").transform.position; //kinda jank
        }

        if (SpawnInGrid)
        {
            Tilemap map = GameObject.Find("Clickable").GetComponent<Tilemap>();
            pos = map.WorldToCell(pos) + new Vector3(0.5f,0.5f,0f);
        }

        Object.Instantiate(prefab, pos, Quaternion.identity);
    }
}

[System.Serializable]
public class TileSpawnerItem : SpawnerItem
{
    [SerializeField]
    private string TileSlug;
    private RuleTile tile;

    public TileSpawnerItem(int id, string title, int value, string slug) : base(id, title, value, slug) {}

    protected override void Interact<T>(T obj)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        //This map is temp, maybe a slug to find correct map to place into?
        Tilemap map = GameObject.Find("Clickable").GetComponent<Tilemap>();

        Vector3Int cellPos = map.WorldToCell(pos);

        map.SetTile(cellPos, tile);
    }

    protected override void ExtendInit()
    {
        this.tile = Resources.Load<RuleTile>("Sprites/RuleTiles/" + TileSlug);
        cursor = GameObject.Find("UI/Canvas/Cursor").GetComponent<Cursor>();
    }
}
