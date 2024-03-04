using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{


    public string openDoorAnimName;
    public string closeDoorAnimName;
    private Animator m_animator;
    public UnlockLevel m_doorLockLevel;
    public Inventory inventory;
    public bool isDoorOpen = false;
    private bool isUnlocked = false;
    public NotificationPresenter notificationPresenter;
    private AudioSource m_audioSource;


    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = transform.parent.GetComponent<AudioSource>();
        m_animator = transform.parent.GetComponent<Animator>();
    }


    public void interactWithDoor(Characters character)
    {
        // Nobody can open it while its locked
        if (!isUnlocked)
        {
            if (inventory.itemList[inventory.currentItemCursor] != null)
            {
                if (((Item)inventory.itemList[inventory.currentItemCursor]).itemTypeID == 1)
                {
                    if(((DoorKey)inventory.itemList[inventory.currentItemCursor]).keyColor == m_doorLockLevel)
                    {
                        // Unlock
                        isUnlocked = true;
                        inventory.destroyItemAtSlot(inventory.currentItemCursor);

                    }
                }

            }
            else
            {
                // Missclick
                var notify = new Notification();
                notify.topic = "Locked Door";
                notify.description = string.Format("You need {0} key to unlock this door.", m_doorLockLevel.ToString());
                notificationPresenter.addNotification(notify);
            }

        }

        if (isUnlocked)
        {
            m_audioSource.enabled = true;
            m_audioSource.Play();

            isDoorOpen = !isDoorOpen;


            if (isDoorOpen)
                m_animator.Play(openDoorAnimName);
            else
                m_animator.Play(closeDoorAnimName);
        }

    }


        
    void Update()
    {


    }
}
