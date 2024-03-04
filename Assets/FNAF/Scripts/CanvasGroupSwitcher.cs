using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupSwitcher : MonoBehaviour
{

    public GameObject HUD;
    public GameObject CameraInterface;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setUI(UserInterfaces cui)
    {
        if (cui == UserInterfaces.None)
        {
            HUD.SetActive(false);
            CameraInterface.SetActive(false);
        }
        else if (cui == UserInterfaces.HUD)
        {
            HUD.SetActive(true);
            CameraInterface.SetActive(false);
        }
        else if (cui == UserInterfaces.CameraInterface)
        {
            HUD.SetActive(false);
            CameraInterface.SetActive(true);
        }
    }


    public void hideHUD()
    {
        HUD.SetActive(false);
    }
    public void showHUD()
    {
        HUD.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
