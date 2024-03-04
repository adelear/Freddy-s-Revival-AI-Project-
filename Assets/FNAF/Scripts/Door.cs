using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{

    public string openDoorAnimName;
    public string closeDoorAnimName;
    private Animator m_animator;
    public bool isDoorOpen = false;
    private bool isStrongDoor = false;
    private NavMeshObstacle navBarrier;
    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = transform.parent.GetComponent<AudioSource>();

        TryGetComponent(out navBarrier);

        m_animator = transform.parent.GetComponent<Animator>();
        isStrongDoor = transform.CompareTag("strongDoor");

        if (isDoorOpen)
            m_animator.Play(openDoorAnimName);

    }

    public void interactWithDoor(Characters character)
    {
        m_audioSource.enabled = true;
        m_audioSource.Play();

        if (isStrongDoor)
        {
            // Only player can open strong doors
            if (character == Characters.player)
            {
                isDoorOpen = !isDoorOpen;
            }
        }
        else
        {
            // Player and Freddy can open any door except strong ones
            isDoorOpen = !isDoorOpen;
            if (character == Characters.freddy)
            {
                isDoorOpen = true;
            }
        }

        if (isDoorOpen)
            m_animator.Play(openDoorAnimName);
        else
            m_animator.Play(closeDoorAnimName);

    }

    // Update is called once per frame
    void Update()
    {
        if (navBarrier != null)
            navBarrier.enabled = !isDoorOpen;
    }
}
