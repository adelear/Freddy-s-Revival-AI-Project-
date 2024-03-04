using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public TextMeshProUGUI ih_tmpro1;
    public TextMeshProUGUI ih_tmpro2;
    public Image ih_imgview1;
    public Image ih_imgview2;

    public Sprite chosenItemFrame;
    public Sprite defaultItemFrame;
    public Image itemSlot1Frame;
    public Image itemSlot2Frame;

    public Sprite defaultItemSprite;
    public Object[] itemList = new Object[2];
    public int currentItemCursor = 0;

    private GameObject player;
    public bool isInventoryFull = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Returns success
    public bool addItem(Object item)
    {
        if (item is not Item)
        {
            Debug.LogWarning("Tried to add non-item object to itemList at inventory");
            return false;
        }


        if (itemList[0] == null)
            itemList[0] = item;
        else if (itemList[1] == null)
            itemList[1] = item;
        else
            // Inventory is full
            return false;

        return true;
    }

    public void destroyItemAtSlot(int slotIndex)
    {
        var sobj = itemList[slotIndex];
        Destroy(sobj);
        itemList[slotIndex] = null;
    }


    // Update is called once per frame
    void Update()
    {

        isInventoryFull = itemList[0] != null && itemList[1] != null;

        // Item item = (Item)itemList[i];
        for (int slot = 0; slot < 2; slot++)
        {
            // Item aItem = (Item)itemList[slot];
            if (slot == 0)
            {
                ih_tmpro1.text = itemList[slot] != null ? ((Item)itemList[slot]).itemName : "Blank";
                ih_imgview1.sprite = itemList[slot] != null ? ((Item)itemList[slot]).itemSprite : defaultItemSprite;
            }
            else
            {
                ih_tmpro2.text = itemList[slot] != null ? ((Item)itemList[slot]).itemName : "Blank";
                ih_imgview2.sprite = itemList[slot] != null ? ((Item)itemList[slot]).itemSprite : defaultItemSprite;
            }
        }
                

        // Apply inventory cursor controls
        if (Input.GetButtonDown("Switch to Inventory Slot 1"))
        {
            currentItemCursor = 0;
        }
        else if (Input.GetButtonDown("Switch to Inventory Slot 2"))
        {
            currentItemCursor = 1;
        }


        // Set the selected slot ui

        itemSlot1Frame.sprite = currentItemCursor == 0 ? chosenItemFrame: defaultItemFrame;
        itemSlot2Frame.sprite = currentItemCursor == 1 ? chosenItemFrame : defaultItemFrame;



        if (Input.GetButtonDown("Drop Item"))
        {
            if (itemList.Length -1 >= currentItemCursor)
                if (itemList[currentItemCursor] != null)
                {
                    uint itemType = ((Item)itemList[currentItemCursor]).itemTypeID;

                    switch (itemType)
                    {
                        case 0:
                            {
                                ((Tape)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }

                        case 1:
                            {
                                ((DoorKey)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }
                        case 2:
                            {
                                ((Drink)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }
                        case 3:
                            {
                                ((Pizza)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }
                        case 4:
                            {
                                ((PlasticPlate)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }
                        case 5:
                            {
                                ((PlasticFork)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }
                        case 6:
                            {
                                ((PartyHat)itemList[currentItemCursor]).dropIt(player);
                                itemList[currentItemCursor] = null;
                                break;
                            }

                        default:
                            {
                                Debug.Log(string.Format("typeID of item at slot {} has no defined action", currentItemCursor));
                                break;
                            }

                    }

                }
        }




    }
}
