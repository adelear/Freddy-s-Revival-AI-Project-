using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject contactMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onPlay()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void onContactOpen()
    {
        contactMenu.SetActive(true);
    }

    public void onContactClose()
    {
        contactMenu.SetActive(false);
    }

    public void onExit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
