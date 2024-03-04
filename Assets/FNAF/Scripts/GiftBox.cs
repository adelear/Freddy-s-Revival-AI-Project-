using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{

    public GameObject ribbon;
    private Collider m_collider;

    public Animator giftLCoverAnim;
    public Animator giftRCoverAnim;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
    }


    public void open()
    {

        m_collider.enabled = false;
        Rigidbody rrb = ribbon.AddComponent<Rigidbody>();
        rrb.AddForce((transform.up + transform.right) * 300);

        giftLCoverAnim.Play("giftbox_l_open");
        giftRCoverAnim.Play("giftbox_r_open");

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
