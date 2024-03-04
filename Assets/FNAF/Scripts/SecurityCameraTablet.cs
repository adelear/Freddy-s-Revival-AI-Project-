using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraTablet : MonoBehaviour
{

    public CanvasGroupSwitcher CGSwitcher;
    public GameObject playerCameraObj;
    public GameObject[] cameraList = new GameObject[8];
    public PlayerCamera playerCameraScr;
    public PlayerController playerControllerScr;
    public GameObject PlayerFlashlight;
    public Animator CameraTVNoise;
    private int cameraIndex = -1;
    private AudioSource m_audioSource;
    private AudioListener m_audioListener;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioListener = GetComponent<AudioListener>();
    }

    private void removeAllSCamsFromViewport()
    {
        foreach (var cam in cameraList)
            cam.SetActive(false);
    }

    public void switchSCamera(int idx)
    {
        cameraIndex = idx;
        removeAllSCamsFromViewport();
        m_audioSource.enabled = true;
        m_audioSource.Play();
    }

    public void playerInteract()
    {
        m_audioListener.enabled = true;
        CGSwitcher.setUI(UserInterfaces.CameraInterface);
        playerCameraScr.lockCursor = false;
        playerControllerScr.lockPlayerController = true;
        playerCameraObj.SetActive(false);
        PlayerFlashlight.SetActive(false);
        CameraTVNoise.Play("tvnoise");
        cameraIndex = 0;
    }

    public void closeCameraInterface()
    {
        m_audioListener.enabled = false;
        cameraIndex = -1;
        playerCameraObj.SetActive(true);
        PlayerFlashlight.SetActive(true);
        playerCameraScr.lockCursor = true;
        playerControllerScr.lockPlayerController = false;
        CGSwitcher.setUI(UserInterfaces.HUD);
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraIndex != -1)
            cameraList[cameraIndex].SetActive(true);
        else
            removeAllSCamsFromViewport();

    }
}
