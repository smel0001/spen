using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class SpawnerItem : InteractorItem
{
    #region Required Fields
    public string PrefabSlug;
    #endregion

    #region Optional Fields
    [System.ComponentModel.DefaultValue(true)]
    public bool SpawnAtCursor;
    [System.ComponentModel.DefaultValue(true)]
    public bool SpawnInGrid;
    [System.ComponentModel.DefaultValue(0.0f)]
    public float SpawnCooldown;
    [System.ComponentModel.DefaultValue(false)]
    public bool SpawnSingleton;
    #endregion

    #region Internal Fields
    private GameObject prefab;
    private bool spawnUnlock = true;
    private float cdTimer;
    private GameObject singleton;
    #endregion
    
    public SpawnerItem(int id, string title, int value, string slug) : base(id, title, value, slug)
    {}

    protected override void ExtendInit()
    {
        this.prefab = Resources.Load<GameObject>("Prefabs/" + PrefabSlug);
        cursor = GameObject.Find("UI/Canvas/Cursor").GetComponent<Cursor>();
        cdTimer = SpawnCooldown;
    }

    protected override bool Interact<T>(T obj)
    {
        //Pre Handle Options

        //TODO: fix singleton spawning
        //if (SpawnSingleton && singleton != null) { return; }
        if (!spawnUnlock) { return false; }

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

        bool wasSpawned;
        if (SpawnInGrid)
        {
            wasSpawned = ObjectManager.Instance.AddObjectToGrid(prefab, pos);
        }
        else
        {
            wasSpawned = ObjectManager.Instance.AddObject(prefab, pos);
        }

        if (wasSpawned)
        {
            //Post Handle Options
            /*
            if (SpawnSingleton)
            {
                singleton = spawnedObj;
            }
            */
            spawnUnlock = false;
            cdTimer = SpawnCooldown;
        }

        return wasSpawned;
    }

    protected override void Update()
    {
        
        if (SpawnCooldown > 0)
        {
            if (cdTimer <= 0)
            {
                spawnUnlock = true;
            }
            else
            {
                cdTimer -= Time.deltaTime;
            }
        }
        else
        {
            spawnUnlock = true;
        }
    }
}

[System.Serializable]
public class TileSpawnerItem : SpawnerItem
{
    #region Required Fields
    public string TileSlug;
    #endregion

    #region Internal Fields
    private RuleTile tile;
    #endregion

    public TileSpawnerItem(int id, string title, int value, string slug) : base(id, title, value, slug) {}

    protected override bool Interact<T>(T obj)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        //This map is temp, maybe a slug to find correct map to place into?
        Tilemap map = GameObject.Find("Clickable").GetComponent<Tilemap>();

        Vector3Int cellPos = map.WorldToCell(pos);

        map.SetTile(cellPos, tile);
        return true;
    }

    protected override void ExtendInit()
    {
        this.tile = Resources.Load<RuleTile>("Sprites/RuleTiles/" + TileSlug);
        cursor = GameObject.Find("UI/Canvas/Cursor").GetComponent<Cursor>();
    }
}
