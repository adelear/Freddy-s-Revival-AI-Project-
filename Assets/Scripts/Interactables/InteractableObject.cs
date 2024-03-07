using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("General Components")]
    public string Name;
    public bool isPickup; 
    private float interactDistance = 5f;
    private bool isInteracting = false;
    private TMP_Text interactText;
    public InteractableType interactableType;

    protected void Start() 
    { 
        interactText = GameObject.FindGameObjectWithTag("InteractText").GetComponent<TMP_Text>();
        interactText.text = "";
    }

    // When player presses interact, this gets called
    public virtual void Interact()
    {
        StartCoroutine(InteractDone()); 
    }

    IEnumerator InteractDone()
    {
        isInteracting = true;
        switch (interactableType)
        {
            case InteractableType.FakeExit:
                interactText.text = "What kind of emergency exit is locked on the inside...";
                Debug.Log("Interacting with fake exit");
                break;
            case InteractableType.RealExit:
                interactText.text = "I can't leave without the music box...";
                break;
            case InteractableType.Cupcake:
                TaskManager.Instance.CompleteTask(TaskManager.TaskType.CollectCupcake);
                Destroy(gameObject); 
                break;
            case InteractableType.StunGun:
                TaskManager.Instance.CompleteTask(TaskManager.TaskType.FindStunGun);
                Camera.main.transform.Find("StunGun").gameObject.SetActive(true);
                Destroy(gameObject); 
                break;
            default:
                Debug.Log("Interacting");
                break;
        }

        yield return new WaitForSeconds(6.0f);
        interactText.text = "";
        isInteracting = false;
    }

    // Calling the lines such as "Interact with Exit Door, Key, etc"
    public virtual void LookAt()
    {
        if (!isInteracting) interactText.text = isPickup ? $"Press E to Pickup {Name}" : $"Press E to Interact with {Name}"; 
    }

    // Looking away clears the line 
    public virtual void LookAway()
    {
        if (!isInteracting) interactText.text = "";
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
        }
        else LookAway(); 
    }
}
