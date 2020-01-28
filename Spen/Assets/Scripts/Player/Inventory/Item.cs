using UnityEngine;


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
        this.Stackable = false;
        this.Slug = slug;
    }

    public void Init()
    {
        //Core Init
        this.Icon = Resources.Load<Sprite>("Sprites/Items/" + Slug);
        ExtendInit();
    }

    protected virtual void ExtendInit()
    {}

    public string DataString()
    {
        string data = "<color=#ff0000><b>" + this.Title + "</b></color>\n\n" + this.ID;
        return data;
    }

    public void CreateInWorld()
    {
        WorldItem = new GameObject(this.Title);
        SpriteRenderer sr = WorldItem.AddComponent<SpriteRenderer>() as SpriteRenderer;
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
    {
    }

    protected override void ExtendInit()
    { }

    //Equivalent of on right click
    public virtual void Activate()
    {
        Debug.Log("She works");
    }
}

public class PlaceItem : UseItem
{
    public string PrefabSlug;
    private GameObject prefab;

    public PlaceItem(int id, string title, int value, string slug) : base(id, title, value, slug)
    {
        Debug.Log("Make Use");
       
    }

    public override void Activate()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        Object.Instantiate(prefab, pos, Quaternion.identity);
    }

    protected override void ExtendInit()
    {
        PrefabSlug = "chikin";
        this.prefab = Resources.Load<GameObject>("Prefabs/" + PrefabSlug);
        Debug.Log("?");
    }
}