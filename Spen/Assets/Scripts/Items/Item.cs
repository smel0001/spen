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
    
    public string Slug;
    #endregion

    #region Optional Fields
    [System.ComponentModel.DefaultValue(true)]
    public bool Stackable;
    [System.ComponentModel.DefaultValue(0)]
    public int Value;
    #endregion

    #region Internal Fields
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
        this.Icon = Resources.Load<Sprite>("Sprites/Items/" + Slug);
        WorldItem = Resources.Load<GameObject>("Prefabs/WorldItem");
        ExtendInit();
    }

    protected virtual void ExtendInit() {}

    public string DataString()
    {
        string data = "<color=#ff0000><b>" + this.Title + "</b></color>\n\n" + this.ID;
        return data;
    }

    

    public virtual void EnterSelected() {}
    public virtual void WhileSelected()
    {
        this.Update();
    }
    public virtual void ExitSelected() {}

    protected virtual void Update() {}

    public void CreateInWorld(Vector3 pos)
    {
        WorldItem.GetComponent<SpriteRenderer>().sprite = this.Icon;
        WorldItem.GetComponent<WorldItem>().itemId = this.ID;
        ObjectManager.Instance.AddObject(WorldItem, pos);
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