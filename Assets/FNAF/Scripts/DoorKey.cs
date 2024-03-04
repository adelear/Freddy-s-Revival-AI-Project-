using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : Item
{

    public UnlockLevel keyColor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void pickIt()
    {
        onPickUpItem();
    }

    public void dropIt(GameObject refGObj)
    {
        onDropItem(refGObj);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
