using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //UI
    GameObject inventoryPanel;
    GameObject slotPanel;

    Tooltip tooltip;

    GameObject toolbarPanel;
    GameObject toolbarSlotPanel;

    public GameObject inventorySlot;
    public GameObject inventoryItem;

    //
    ItemDatabase itemData;
    private int slotAmount = 36;
    private int toolbarSlotAmount = 9;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    public int SelectedSlot = 0;

    void Start()
    {
        //TODO instance ItemDatabase
        itemData =  GetComponent<ItemDatabase>();
        inventoryPanel = GameObject.Find("InventoryPanel");
        slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;
        tooltip = GetComponent<Tooltip>();

        toolbarPanel = GameObject.Find("ToolbarPanel");
        toolbarSlotPanel = toolbarPanel.transform.Find("ToolbarSlots").gameObject;

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


        AddItem(0);
        AddItem(0);
        AddItem(2);
        AddItem(1);
        AddItem(0);
        AddItem(0);
        AddItem(0);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (items[SelectedSlot] as UseItem != null)
            {
                UseItem activeItem = (UseItem)items[SelectedSlot];
                activeItem.Activate();

                if (activeItem.RemoveAfterUse)
                {
                    RemoveItem(SelectedSlot);
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
            RemoveItem(SelectedSlot);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(SelectedSlot);
            updateSelectedSlot(SelectedSlot+1);
            Debug.Log(SelectedSlot);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            updateSelectedSlot(SelectedSlot-1);
        }

        //Input for selected slot in toolbar
    }

    public void AddItem(int id)
    {
        Item itemToAdd = itemData.FetchItemById(id);

        if (itemToAdd.Stackable)
        {
            int? index = FindItemIndex(id);
            if (index != null)
            {
                ItemData data = slots[index.GetValueOrDefault()].transform.GetComponentInChildren<ItemData>();
                data.amount++;
                data.transform.GetComponentInChildren<Text>().text = (data.amount + 1).ToString();
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
                data.transform.GetComponentInChildren<Text>().text = (data.amount + 1).ToString();
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
    


    public void RemoveItem(int index)
    {
        if (items[index].ID != -1)
        {
            items[index].CreateInWorld();

            //maybe move to some world list
            //set to player pos

            items[index] = new Item();
            Destroy(slots[index].transform.GetChild(0).gameObject);
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
        if (index > toolbarSlotAmount - 1)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = toolbarSlotAmount - 1;
        }

        slots[SelectedSlot].GetComponent<Image>().color = Color.white;
        SelectedSlot = index;
        slots[SelectedSlot].GetComponent<Image>().color = Color.red;
    }
}
