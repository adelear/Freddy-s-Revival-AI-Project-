using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : MonoBehaviour
{

    public string openDoorAnimName;
    public string closeDoorAnimName;
    private Animator m_animator;
    public bool isDoorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = transform.parent.GetComponent<Animator>();

        if (isDoorOpen)
            m_animator.Play(openDoorAnimName);
    }

    public void interactWithDoor()
    {
        isDoorOpen = !isDoorOpen;

        if (isDoorOpen)
            m_animator.Play(openDoorAnimName);
        else
            m_animator.Play(closeDoorAnimName);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
