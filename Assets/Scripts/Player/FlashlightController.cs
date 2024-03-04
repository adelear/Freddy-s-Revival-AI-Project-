using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightController : MonoBehaviour
{
    [Header("Camera Components")]
    [SerializeField] private Vector3 vectOffset;
    [SerializeField] private GameObject cameraFollow;
    [SerializeField] private float speed = 5.0f;

    [Header("Flashlight Components")]
    [SerializeField] private GameObject flashlightObject;
    [SerializeField] private Light flashlight;
    [SerializeField] private bool flashlightOn;
   

    [Header("Intensity and Angle Change")]
    [SerializeField] private float raycastDistance = 10f;
    [SerializeField] private float lowIntensity = 0.25f;
    [SerializeField] private float highIntensity = 0.5f;
    [SerializeField] private float targetIntensity = 0.5f;
    [SerializeField] private float lowAngle = 35f;
    [SerializeField] private float highAngle = 66.5f;
    [SerializeField] private float targetAngle = 35f;
    [SerializeField] private float intensityChangeSpeed = 10.0f; 

    //public AudioClip turnOn;
    //public AudioClip turnOff;
    //public AudioManager asm; 

    void Start()
    {
        cameraFollow = Camera.main.gameObject; 
        flashlightOn = false;
        flashlightObject = transform.Find("Flashlight").gameObject;
        flashlight = flashlightObject.GetComponent<Light>(); 
        vectOffset = flashlightObject.transform.position - cameraFollow.transform.position; 
        flashlightObject.SetActive(false);
    }

    private void DetectObjectInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            targetIntensity = highIntensity;
            targetAngle = highAngle;
            Debug.Log("Object detected in front: " + hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        else
        {
            targetAngle = lowAngle; 
            targetIntensity = lowIntensity; 
            Debug.DrawLine(transform.position, transform.position + transform.forward * raycastDistance, Color.green);
        }
    }
    private void Update()
    {
        if (!flashlightOn && Input.GetMouseButtonDown(0) && GameManager.Instance.GetGameState()==GameManager.GameState.GAME)
        {
            flashlightObject.SetActive(true);
            //asm.PlayOneShot(turnOn, false); 
            flashlightOn =true; 
        }
        else if (flashlightOn && Input.GetMouseButtonDown(0) && GameManager.Instance.GetGameState() == GameManager.GameState.GAME)
        {
            flashlightObject.SetActive(false);
            //asm.PlayOneShot(turnOff, false);
            flashlightOn =false;
        }

        flashlightObject.transform.position = cameraFollow.transform.position + vectOffset;
        flashlightObject.transform.rotation = Quaternion.Slerp(flashlightObject.transform.rotation, cameraFollow.transform.rotation, speed * Time.deltaTime);

        DetectObjectInFront(); 
        flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, Time.deltaTime * intensityChangeSpeed);
        flashlight.innerSpotAngle = Mathf.Lerp(flashlight.innerSpotAngle, targetAngle, Time.deltaTime * intensityChangeSpeed);
        flashlight.shadowAngle = Mathf.Lerp(flashlight.shadowAngle, targetAngle, Time.deltaTime * 1.0f);
        //flashlight.spotAngle = Mathf.Lerp(flashlight.spotAngle, targetAngle, Time.deltaTime * intensityChangeSpeed);
    }
}
