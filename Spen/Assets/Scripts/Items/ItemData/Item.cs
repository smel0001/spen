/*
This class describes the base functionality of an Item in the game.
It is also serializable to allow for loading in the Item database.
*/
using UnityEngine;
using UnityEngine.Tilemaps;

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
        WorldItem = Resources.Load<GameObject>("Prefabs/Objects/WorldItem");
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

/*
Usable Item is any item that the player can use from their inventory via right clicking while it is selected.
(12/3/2020 no instances of this item implemmented yet).
*/
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