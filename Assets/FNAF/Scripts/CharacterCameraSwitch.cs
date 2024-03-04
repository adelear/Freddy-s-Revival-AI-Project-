using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCameraSwitch : MonoBehaviour
{

    public GameObject freddyCamera;
    public GameObject bonnieCamera;
    public GameObject chicaCamera;
    public GameObject endoskeletonCamera;
    public GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void switchCamera(Characters character)
    {
        if (character == Characters.player)
        {
            freddyCamera.SetActive(false);
            bonnieCamera.SetActive(false);
            chicaCamera.SetActive(false);
            endoskeletonCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        else if (character == Characters.endoskeleton)
        {
            freddyCamera.SetActive(false);
            bonnieCamera.SetActive(false);
            chicaCamera.SetActive(false);
            endoskeletonCamera.SetActive(true);
            playerCamera.SetActive(false);
        }
        else if (character == Characters.freddy)
        {
            freddyCamera.SetActive(true);
            bonnieCamera.SetActive(false);
            chicaCamera.SetActive(false);
            endoskeletonCamera.SetActive(false);
            playerCamera.SetActive(false);
        }
        else if (character == Characters.chica)
        {
            freddyCamera.SetActive(false);
            bonnieCamera.SetActive(false);
            chicaCamera.SetActive(true);
            endoskeletonCamera.SetActive(false);
            playerCamera.SetActive(false);
        }
        else if (character == Characters.bonnie)
        {
            freddyCamera.SetActive(false);
            bonnieCamera.SetActive(true);
            chicaCamera.SetActive(false);
            endoskeletonCamera.SetActive(false);
            playerCamera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
