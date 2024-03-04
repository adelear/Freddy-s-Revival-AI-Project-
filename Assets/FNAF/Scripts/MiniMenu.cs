using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMenu : MonoBehaviour
{

    public GameObject miniMenu;
    public GameObject contactPage;
    public PlayerCamera playerCamera;
    public PlayerController playerController;
    public DeadScreen deadScreen;
    private bool isMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onPlay()
    {
        isMenuOpen = false;
        playerCamera.lockCursor = true;
    }
    public void onContactOpen()
    {
        contactPage.SetActive(true);
    }
    public void onContactClose()
    {
        contactPage.SetActive(false);
    }
    public void onExit()
    {
        isMenuOpen = false;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isMenuOpen = !isMenuOpen;
        }

        if (deadScreen.isDead)
            isMenuOpen = false;

        miniMenu.SetActive(isMenuOpen);
        if (!isMenuOpen)
            contactPage.SetActive(false);
        playerCamera.lockCamera = isMenuOpen;
        if (isMenuOpen)
            playerCamera.lockCursor = false;
        playerController.lockPlayerController = isMenuOpen;
        Time.timeScale = isMenuOpen ? 0 : 1;
    }
}
