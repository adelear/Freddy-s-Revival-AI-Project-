using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoubleDoor : MonoBehaviour
{
    public string openDoorAnimName_r;
    public string closeDoorAnimName_r;
    public string openDoorAnimName_l;
    public string closeDoorAnimName_l;

    public Animator animator_r;
    public Animator animator_l;

    public bool isDoorOpen = false;
    private bool isStrongDoor = false;

    private NavMeshObstacle navBarrier;
    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        TryGetComponent(out navBarrier);

        isStrongDoor = transform.CompareTag("strongDoor");

        if (isDoorOpen)
        {
            animator_r.Play(openDoorAnimName_r);
            animator_l.Play(openDoorAnimName_l);
        }
    }

    public void interactWithDoor(Characters character)
    {
        m_audioSource.enabled = true;
        m_audioSource.Play();
        if (isStrongDoor)
        {
            if (character == Characters.player)
            {
                isDoorOpen = !isDoorOpen;
            }
        }
        else
        {
            isDoorOpen = !isDoorOpen;
            if (character == Characters.freddy)
            {
                isDoorOpen = true;
            }
        }

        if (isDoorOpen)
        {
            animator_r.Play(openDoorAnimName_r);
            animator_l.Play(openDoorAnimName_l);

        }
        else
        {
            animator_r.Play(closeDoorAnimName_r);
            animator_l.Play(closeDoorAnimName_l);
        }

    }



    // Update is called once per frame
    void Update()
    {
        if (navBarrier != null)
            navBarrier.enabled = !isDoorOpen;
    }
}
