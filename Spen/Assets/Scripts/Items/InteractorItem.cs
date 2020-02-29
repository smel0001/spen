using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class InteractorItem : UsableItem
{
    public string InteractionTag;

    protected Cursor cursor;

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
            Interact<object>(null);
            return true;
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
                        Interact(tile);
                        return true;
                    }
                }
                if (hit.transform.tag == InteractionTag)
                {
                    Interact(hit.transform.gameObject);
                    return true;
                }
            }
        }
        return false;
    }
    
    protected virtual void Interact<T>(T obj)
    {
        if (obj != null)
        {
            //something like this, cause change in world obj
            //obj.GetComponent<InteractableWorldObject?>().Interact();
        }
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
