using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightButton : MonoBehaviour
{

    public Light targetLight;
    public Light switchLight;
    private Animator switchAnim;
    private bool isOn = false;
    public float lightOnTimerSeconds = 3;
    private float lightOnTimerSecondsClone = 0;
    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        switchAnim = GetComponent<Animator>();
        targetLight.enabled = false;
    }
    public void clickButton()
    {
        m_audioSource.enabled = true;
        m_audioSource.Play();
        if (!isOn)
        {
            // Light is not active
            isOn = true;
            targetLight.enabled = true;
            switchAnim.Play("doorlight_on_off", 0, 0);
            switchLight.color = Color.green;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            lightOnTimerSecondsClone += Time.deltaTime;
            if (lightOnTimerSecondsClone >= lightOnTimerSeconds)
            {
                // Turn off the light

                isOn = false;
                targetLight.enabled = false;
                switchLight.color = Color.red;
                lightOnTimerSecondsClone = 0;
            }
        }
    }
}
