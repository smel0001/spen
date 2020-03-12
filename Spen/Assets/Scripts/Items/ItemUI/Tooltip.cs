/*
UI Tooltip element to show item data in inventory
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void Activate(Item item)
    {
        GetComponentInChildren<Text>().text = item.DataString();
        transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
