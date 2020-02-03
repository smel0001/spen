using UnityEngine;
using UnityEngine.Tilemaps;

/*
Options for Item sub-classes:

Seperate .json for each sub-class. Item ends up just being an interface.
Some function that needs to be aware of all sub-types so all can be init'd.
    - another option is that Item is not an interface and is general.

Field in Item as definition then create another class? (feels jank).

*/
[System.Serializable]
public class Item
{
    public int ID;
    public string Title;
    public int Value;
    public bool Stackable;
    public string Slug;

    [System.NonSerialized]
    public Sprite Icon;
    private GameObject WorldItem;

    public Item()
    {
        this.ID = -1;
    }

    public Item(int id, string title, int value, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Stackable = true;
        this.Slug = slug;
    }

    public void Init()
    {
        //Core Init
        this.Icon = Resources.Load<Sprite>("Sprites/Items/" + Slug);
        WorldItem = Resources.Load<GameObject>("Prefabs/WorldItem");
        ExtendInit();
    }

    protected virtual void ExtendInit()
    {}

    public string DataString()
    {
        string data = "<color=#ff0000><b>" + this.Title + "</b></color>\n\n" + this.ID;
        return data;
    }

    public virtual void EnterSelected() {}
    public virtual void WhileSelected() {}
    public virtual void ExitSelected() {}

    public void CreateInWorld(Vector3 pos)
    {
        GameObject myObj = Object.Instantiate(WorldItem, pos, Quaternion.identity);
        SpriteRenderer sr = myObj.GetComponent<SpriteRenderer>();
        myObj.GetComponent<WorldItem>().itemId = this.ID;
        sr.sprite = this.Icon;
    }

    public void RemoveFromWorld()
    {
        Object.Destroy(WorldItem);
    }
}

public class EquipItem : Item
{
    //Equip stats
}

[System.Serializable]
public class UseItem : Item
{
    //Will probably need subclasses
    //Consume
    //Weapon
    //Ranged
    //Melee
    //Tool
    //?
    public bool RemoveAfterUse = true;

    public UseItem(int id, string title, int value, string slug) : base(id, title, value, slug)
    {}

    protected override void ExtendInit()
    {}

    //Equivalent of on right click
    public virtual bool Activate()
    {
        return false;
    }
}

[System.Serializable]
public class PlaceItem : UseItem
{
    public string PlaceOnTag;
    public string PrefabSlug;
    private GameObject prefab;

    //TEMP, find better way to do this, maybe as static etc
    private Cursor cursor;

    //Icon image for indicator

    public PlaceItem(int id, string title, int value, string slug) : base(id, title, value, slug)
    {}

    public override bool Activate()
    {
        if (PlaceOnTag != "")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit)
            {
                if (hit.transform.tag == PlaceOnTag)
                {
                    PlaceInWorld();
                    return true;
                }
                Debug.Log("no match");
            }
            Debug.Log("no hit");
            return false;
        }
        else
        {
            Debug.Log("no tag");
            PlaceInWorld();
            return true;
        }
    }

    private void PlaceInWorld()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        Tilemap map = GameObject.Find("Deco").GetComponent<Tilemap>();

        Vector3 cellPos = map.WorldToCell(pos);
        //offset might be jank idk
        cellPos += new Vector3(0.5f, 0.5f, 0);

        Object.Instantiate(prefab, cellPos, Quaternion.identity);
    }

    protected override void ExtendInit()
    {
        this.prefab = Resources.Load<GameObject>("Prefabs/" + PrefabSlug);
        cursor = GameObject.Find("UI/Canvas/Cursor").GetComponent<Cursor>();
    }

    public override void EnterSelected() 
    {
        cursor.SetSprite(this.Icon);
    }
    public override void WhileSelected() {}
    public override void ExitSelected()
    {
        cursor.ResetSprite();
    }


}

/*
public class PlaceTileItem : PlaceItem
use a RuleTile instead of a prefab when placing, and place into the appropriate tilemap layer
*/
