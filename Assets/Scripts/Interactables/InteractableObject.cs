using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public string Name;
    public bool isPickup; 
    private float interactDistance = 4f;
    private bool isInteracting = false; 
    private TMP_Text interactText;

    private InteractableType interactableType;
    public InteractableType InteractableType
    {
        get { return interactableType; }
        set { interactableType = value; }
    }

    protected void Start() 
    { 
        interactText = GameObject.FindGameObjectWithTag("InteractText").GetComponent<TMP_Text>();
        interactText.text = "";
    }

    // When player presses interact, this gets called
    public virtual void Interact()
    {
        isInteracting = true; 
        switch (interactableType)
        {
            case InteractableType.FakeExit:
                interactText.text = "What kind of emergency exit is locked on the inside...";
                break;
            case InteractableType.RealExit:
                Debug.Log("Interacting");
                break;
            case InteractableType.Key:
                Debug.Log("Interacting");
                break;
            case InteractableType.Mask:
                Debug.Log("Interacting");
                break;
            case InteractableType.StunGun:
                Debug.Log("Interacting");
                break;
            default:
                Debug.Log("Interacting"); 
                break;
        }
    }

    // Calling the lines such as "Interact with Exit Door, Key, etc"
    public virtual void LookAt()
    {
        if (!isInteracting) interactText.text = isPickup ? $"Press E to Pickup {Name}" : $"Press E to Interact with {Name}"; 
    }

    // Looking away clears the line 
    public virtual void LookAway()
    {
        interactText.text = "";
        isInteracting = false; 
    }

    public virtual void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.gameObject == this.gameObject)
            {
                LookAt();
                if (Input.GetKeyDown(KeyCode.E)) Interact();
            }
            else LookAway();
        }
        else LookAway(); 
    }
}
