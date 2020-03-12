using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WorldItem : MonoBehaviour
{
    public int itemId;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            //Add to inv
            //Super jank inv should be attached to player but for now we'll survive
            collision.GetComponent<Inventory>().AddItem(itemId);
            ObjectManager.Instance.RemoveObject(this.gameObject.GetInstanceID());
        }
    }
}
