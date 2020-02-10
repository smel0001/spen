/*
 * TODO: Split UI from inv functionality and place UI in a seperate script
 * or don't
 * goal is to have the inv with the player 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //UI
    GameObject inventoryPanel;
    GameObject slotPanel;
    
    [System.NonSerialized]
    public Tooltip tooltip;

    GameObject toolbarPanel;
    GameObject toolbarSlotPanel;

    //UI Prefabs
    private GameObject inventorySlot;
    private GameObject inventoryItem;

    private int slotAmount = 36;
    private int toolbarSlotAmount = 9;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    public int SelectedSlot = 0;

    void Start()
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
        slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;
        inventoryPanel.SetActive(false);

        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        tooltip.Deactivate();

        toolbarPanel = GameObject.Find("ToolbarPanel");
        toolbarSlotPanel = toolbarPanel.transform.Find("ToolbarSlots").gameObject;

        inventorySlot = Resources.Load<GameObject>("Prefabs/UI/Slot");
        inventoryItem = Resources.Load<GameObject>("Prefabs/UI/Item");

        for (int i = 0; i < toolbarSlotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot, toolbarSlotPanel.transform));
            slots[i].GetComponent<ItemSlot>().index = i;
        }

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot, slotPanel.transform));
            slots[i + toolbarSlotAmount].GetComponent<ItemSlot>().index = i + toolbarSlotAmount;
        }

        updateSelectedSlot(0);


        //Load saved items, can just be a list of ids and amounts somewhere (for now)
        AddItem(6);
        AddItem(6);
        AddItem(6);
        AddItem(6);
        AddItem(6);
        AddItem(5);
    }

    //Mostly temp
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (items[SelectedSlot] as UseItem != null)
            {
                UseItem activeItem = (UseItem)items[SelectedSlot];
                if (activeItem.Activate())
                {
                    if (activeItem.RemoveAfterUse)
                    {
                        RemoveItem(SelectedSlot);
                    }
                }
            }
            else
            {
                //raytest
                //All this has to move to like player or something
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit)
                {
                    if (hit.collider != null)
                    {
                        Debug.Log(hit.transform.gameObject.name);
                        GameObject obj = hit.transform.gameObject;
                        if (obj.tag == "Clickable")
                        {
                            obj.GetComponent<Plant>().OnCast();
                            //Need a series of classes for objects in world, with on clicked() etc.
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            tooltip.Deactivate();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Drop selected item
            DropItem(SelectedSlot);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            updateSelectedSlot(SelectedSlot+1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            updateSelectedSlot(SelectedSlot-1);
        }

        //Input for selected slot in toolbar
        //TODO add on selected and exit selected
        items[SelectedSlot].WhileSelected();

        
    }

    public void AddItem(int id)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemById(id);
        if (itemToAdd.Stackable)
        {
            int? index = FindItemIndex(id);
            if (index != null)
            {
                ItemData data = slots[index.GetValueOrDefault()].transform.GetComponentInChildren<ItemData>();
                data.amount++;
                data.UpdateAmountText();
                return;
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == -1)
            {
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem, slots[i].transform);
                ItemData objItemData = itemObj.GetComponent<ItemData>();
                objItemData.item = itemToAdd;
                objItemData.slotIndex = i;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Icon;
                itemObj.name = itemToAdd.Title;
                break;
            }
        }
    }

    //DEBUG FUNCTION
    public void AddItem(Item itemToAdd)
    {
        if (itemToAdd.Stackable)
        {
            int? index = FindItemIndex(itemToAdd.ID);
            if (index != null)
            {
                ItemData data = slots[index.GetValueOrDefault()].transform.GetComponentInChildren<ItemData>();
                data.amount++;
                data.UpdateAmountText();
                return;
            }
        }


        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == -1)
            {
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem, slots[i].transform);
                itemObj.GetComponent<ItemData>().item = itemToAdd;
                itemObj.GetComponent<ItemData>().slotIndex = i;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Icon;
                itemObj.name = itemToAdd.Title;
                break;
            }
        }
    }
    
    public void DropItem(int index)
    {
        if (items[index].ID != -1)
        {
            //TEMP
            items[index].CreateInWorld(Vector3.zero);
            //maybe move to some world list
            //set to player pos
            RemoveItem(index);
        }
    }

    public void RemoveItem(int index)
    {
        if (items[index].ID != -1)
        {
            ItemData myItem = slots[SelectedSlot].transform.GetComponentInChildren<ItemData>();
            Debug.Log(myItem.amount);
            if (myItem.amount == 0)
            {
                items[SelectedSlot].ExitSelected();
                items[index] = new Item();
                Destroy(slots[index].transform.GetChild(0).gameObject);
            }
            else
            {
                myItem.amount--;
                myItem.UpdateAmountText();
            }
        }
    }

    private int? FindItemIndex(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == id)
                return i;
        }
        return null;
    }

    private void updateSelectedSlot(int index)
    {
        //clamp
        if (index > toolbarSlotAmount - 1)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = toolbarSlotAmount - 1;
        }

        slots[SelectedSlot].GetComponent<Image>().color = Color.white;
        items[SelectedSlot].ExitSelected();

        SelectedSlot = index;

        slots[SelectedSlot].GetComponent<Image>().color = Color.red;
        items[SelectedSlot].EnterSelected();
    }
}
