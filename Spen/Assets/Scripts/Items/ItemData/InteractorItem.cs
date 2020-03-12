/*
InteractorItem is any UsableItem that may interact with the world.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class InteractorItem : UsableItem
{
    #region Optional Fields
    [System.ComponentModel.DefaultValue("")]
    public string InteractionTag;
    #endregion

    #region Internal Fields
    protected Cursor cursor; //TODO make cursor a static reference in a UI script (or instance cursor into singleton)
    #endregion

    public InteractorItem(int id, string title, int value, string slug) : base(id, title, value, slug)
    {}

    protected override void ExtendInit()
    {
        cursor = GameObject.Find("UI/Canvas/Cursor").GetComponent<Cursor>();
    }

    public override bool Activate()
    {
        if (InteractionTag == "")
        {
            return Interact<object>(null);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, LayerMask.GetMask("Clickable"));
            if (hit)
            {
                if (hit.transform.tag == "Tilemap")
                {
                    Tilemap map = hit.transform.gameObject.GetComponent<Tilemap>();
                    InteractRuleTile tile = map.GetTile(map.WorldToCell(hit.point)) as InteractRuleTile;
                    if (tile.Tag == InteractionTag)
                    {
                        return Interact(tile);
                    }
                }
                if (hit.transform.tag == InteractionTag)
                {
                    return Interact(hit.transform.gameObject);
                }
            }
        }
        return false;
    }
    
    protected virtual bool Interact<T>(T obj)
    {
        if (obj != null)
        {
            //TODO: add worldobjects that can be interacted with
            //e.g.
            //obj.GetComponent<InteractableWorldObject?>().Interact();
        }
        return true;
    }

    //Show icon while selected
    public override void EnterSelected() 
    {
        cursor.SetSprite(this.Icon);
    }
    public override void ExitSelected()
    {
        cursor.ResetSprite();
    }
}
