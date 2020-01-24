using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour,
IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Item item;
    public int amount;
    public int slotIndex;

    private Inventory inv;
    private Vector2 offset;

    void Start()
    {
        inv = GameObject.Find("Inv").GetComponent<Inventory>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            offset = eventData.position - (Vector2)this.transform.position;

            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject castHit = eventData.pointerCurrentRaycast.gameObject;
        if (castHit)
        {
            if (castHit.GetComponent<ItemData>() != null)
            {
                ItemData slotItem = castHit.GetComponent<ItemData>();

                Item hold = inv.items[slotItem.slotIndex];
                int holdIndex = slotItem.slotIndex;

                inv.items[slotItem.slotIndex] = inv.items[slotIndex];
                inv.items[slotIndex] = hold;

                slotItem.slotIndex = slotIndex;
                slotItem.MoveItemSlot();

                slotIndex = holdIndex;
                MoveItemSlot();
            }
            else if (castHit.GetComponent<ItemSlot>())
            {
                ItemSlot newSlot = castHit.GetComponent<ItemSlot>();
                inv.items[newSlot.index] = inv.items[slotIndex];
                inv.items[slotIndex] = new Item();
                slotIndex = newSlot.index;
            }
        }
        
        MoveItemSlot();
        GetComponent<Image>().raycastTarget = true;
    }

    public void MoveItemSlot()
    {
        GameObject newSlot = inv.slots[slotIndex];
        this.transform.SetParent(newSlot.transform);
        this.transform.position = newSlot.transform.position;
    }
}
