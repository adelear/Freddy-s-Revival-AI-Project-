using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{

    public Light switchLight;
    private Animator switchAnim;
    public Animator doorAnimator;
    public string doorOpenAnim;
    public string doorCloseAnim;
    public bool isOn = false;
    public float doorClosedTimerSeconds = 20;
    private float doorClosedTimerSecondsClone;
    public TMPro.TextMeshProUGUI countdownText;
    private AudioSource m_audioSourceDoor;
    private AudioSource m_audioSourceButton;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSourceDoor = GetComponents<AudioSource>()[0];
        m_audioSourceButton = GetComponents<AudioSource>()[1];

        switchAnim = GetComponent<Animator>();
        if (isOn)
            doorAnimator.Play(doorOpenAnim);

        doorClosedTimerSecondsClone = doorClosedTimerSeconds;
    }

    public void clickButton()
    {
        m_audioSourceButton.enabled = true;
        m_audioSourceButton.Play();
        if (doorClosedTimerSecondsClone < doorClosedTimerSeconds && !isOn)
            return;
        switchAnim.Play("doorButton", 0, 0);
        isOn = !isOn;
        if (isOn)
        {
            m_audioSourceDoor.enabled = true;
            m_audioSourceDoor.Play();
            doorAnimator.Play(doorOpenAnim);
        }
        else
            doorAnimator.Play(doorCloseAnim);


    }

    // Update is called once per frame
    void Update()
    {

        if (doorClosedTimerSecondsClone < 0)
        {
            isOn = false;
            doorAnimator.Play(doorCloseAnim);
        }

        if (isOn)
        {
            switchLight.color = Color.green;
            doorClosedTimerSecondsClone -= Time.deltaTime;
        }
        else
        {
            switchLight.color = Color.red;
            doorClosedTimerSecondsClone += Time.deltaTime;
            doorClosedTimerSecondsClone = Mathf.Clamp(doorClosedTimerSecondsClone, 0, doorClosedTimerSeconds);
        }

        countdownText.text = Convert.ToInt32(doorClosedTimerSecondsClone).ToString();

    }
}
