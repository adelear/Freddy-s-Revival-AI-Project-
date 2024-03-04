using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : Item
{
    private Rigidbody m_rigidbody;



    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void pickIt()
    {
        onPickUpItem();
    }

    public void dropIt(GameObject refTransform)
    {
        onDropItem(refTransform);
        var throwForce = 700f;
        transform.eulerAngles = new Vector3(Random.Range(-90, 90),
                                           Random.Range(-90, 90),
                                           Random.Range(-90, 90));
        m_rigidbody.AddForce(refTransform.transform.forward * throwForce);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
