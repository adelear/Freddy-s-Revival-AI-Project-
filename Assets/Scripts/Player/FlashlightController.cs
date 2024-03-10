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
    private float raycastDistance = 10f;
    private float lowIntensity = 0.25f;
    private float highIntensity = 0.5f;
    private float targetIntensity = 0.5f;
    private float lowAngle = 50f;
    private float highAngle = 66.5f;
    private float targetAngle = 50f;
    private float lowRange = 35f;
    private float highRange = 60f;
    private float targetRange = 35f; 
    private float intensityChangeSpeed = 10.0f;

    [Header("Battery Components")]
    public float maxBattery = 100f;
    private float currentBattery;

    public AudioClip turnOn;
    private AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
        cameraFollow = Camera.main.gameObject; 
        flashlightOn = false;
        flashlightObject = transform.Find("Flashlight").gameObject;
        flashlight = flashlightObject.GetComponent<Light>(); 
        vectOffset = flashlightObject.transform.position - cameraFollow.transform.position; 
        flashlightObject.SetActive(false);

        currentBattery = maxBattery; 
    }

    private void DetectObjectInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            targetIntensity = highIntensity;
            targetAngle = highAngle;
            targetRange = lowRange;
            //Debug.Log("Object detected in front: " + hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        else
        {
            targetAngle = lowAngle;
            targetRange = highRange; 
            targetIntensity = lowIntensity; 
            Debug.DrawLine(transform.position, transform.position + transform.forward * raycastDistance, Color.green);
        }
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.clip = turnOn;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            AudioManager.Instance.PlayOneShot(turnOn, false); 
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.GAME)
        {
            if (!flashlightOn && Input.GetMouseButtonDown(0))
            {
                flashlightObject.SetActive(true);
                PlaySound(); 
                flashlightOn = true;
            }
            else if (flashlightOn && Input.GetMouseButtonDown(0))
            {
                flashlightObject.SetActive(false);
                PlaySound(); 
                flashlightOn = false;
            }

            flashlightObject.transform.position = cameraFollow.transform.position + vectOffset;
            flashlightObject.transform.rotation = Quaternion.Slerp(flashlightObject.transform.rotation, cameraFollow.transform.rotation, speed * Time.deltaTime);

            DetectObjectInFront();

            flashlight.innerSpotAngle = Mathf.Lerp(flashlight.innerSpotAngle, targetAngle, Time.deltaTime * intensityChangeSpeed);
            //flashlight.shadowAngle = Mathf.Lerp(flashlight.shadowAngle, targetAngle, Time.deltaTime * 1.0f);
            flashlight.spotAngle = Mathf.Lerp(flashlight.spotAngle, targetAngle, Time.deltaTime * 0.5f);
            flashlight.range = Mathf.Lerp(flashlight.range, targetRange, Time.deltaTime * 1.0f);

            if (Time.time % 10f < Time.deltaTime)
            {
                float randomIntensity = Random.Range(lowIntensity, highIntensity);
                flashlight.intensity = randomIntensity;
            }
            else
            {
                flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, Time.deltaTime * intensityChangeSpeed);
            }
        }
    }
}
