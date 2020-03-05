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
    #region Required Fields
    public int ID;
    public string Title;
    public int Value;
    public bool Stackable;
    public string Slug;
    #endregion

    #region Interal Fields
    [System.NonSerialized]
    public Sprite Icon;
    private GameObject WorldItem;
    #endregion

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
        Debug.Log("Core Init");
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

[System.Serializable]
public class UsableItem : Item
{
    #region Optional Fields
    [System.ComponentModel.DefaultValue(true)]
    public bool RemoveAfterUse;
    #endregion
    
    public virtual bool Activate() {return false;}
    public UsableItem(int id, string title, int value, string slug) : base(id, title, value, slug) {}
}