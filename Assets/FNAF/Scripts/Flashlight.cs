using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    public GameObject lightSource;
    public GameObject[] batteryParts;
    public bool isLightActive = true;

    public float batteryLifeSeconds = 40;
    private float batteryLifeSecondsClone;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        batteryLifeSecondsClone = batteryLifeSeconds;
    }
    
    public void addBatterySeconds(float seconds)
    {
        batteryLifeSecondsClone += seconds;
        batteryLifeSecondsClone = Mathf.Clamp(batteryLifeSecondsClone, 0, batteryLifeSeconds);
    }

    public void triggerFlashlight()
    {
        m_audioSource.enabled = true;
        m_audioSource.Play();
        isLightActive = !isLightActive;
    }

    // Update is called once per frame
    void Update()
    {

        if (batteryLifeSecondsClone > 0 && isLightActive)
        {
            batteryLifeSecondsClone -= Time.deltaTime;
        }
        else
        {
            // Battery end
            isLightActive = false;
        }

        if (isLightActive)
        {
            lightSource.SetActive(true);
        }
        else
        {
            lightSource.SetActive(false);
        }

        float[] batteryPartLimits = new float[batteryParts.Length];
        for (uint i = 0; i < batteryParts.Length; i++)
        {

            batteryPartLimits[i] = batteryLifeSeconds / (i + 2);
            batteryPartLimits[batteryPartLimits.Length - 1] = 0;
        }




        for (uint i = 0; i < batteryPartLimits.Length; i++)
        {
            float lValue = batteryPartLimits[i];
            batteryParts[i].SetActive(batteryLifeSecondsClone > lValue);
        }


    }
}
