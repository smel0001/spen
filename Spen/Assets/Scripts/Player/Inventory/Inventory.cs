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
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            tooltip.Deactivate();
        }
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

    private int? FindItemIndex(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == id)
                return i;
        }
        return null;
    } 
}
