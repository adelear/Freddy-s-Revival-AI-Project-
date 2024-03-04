using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{

    public uint itemTypeID;
    public string itemName;
    public Sprite itemSprite;
    public Inventory inventory;

    protected void onPickUpItem()
    {
        gameObject.SetActive(false);
        inventory.addItem(this);
    }

    protected void onDropItem(GameObject player)
    {
        gameObject.SetActive(true);
        transform.position = player.transform.position + player.transform.forward * 0.4f;
    }


}