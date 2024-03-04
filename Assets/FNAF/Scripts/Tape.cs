using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tape : Item
{
    public AudioClip m_record;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void pickIt()
    {
        onPickUpItem();
    }

    public void dropIt(GameObject refTransform)
    {
        onDropItem(refTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
