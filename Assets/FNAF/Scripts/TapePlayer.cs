using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapePlayer : MonoBehaviour
{

    public Inventory inventory;
    private Tape playingTape;
    public AudioSource m_audioSource;


    public GameObject rProbe;
    public GameObject lProbe;
    public GameObject tapeInTheTapePlayer;



    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }


    public void playerInteractPlay()
    {

        if (inventory.itemList[inventory.currentItemCursor] == null)
            return;

        // Check if item is a tape
        if (((Item)inventory.itemList[inventory.currentItemCursor]).itemTypeID == 0)
        {
            if (!m_audioSource.isPlaying)
            {
                var m_tape = ((Tape)inventory.itemList[inventory.currentItemCursor]);
                inventory.itemList[inventory.currentItemCursor] = null;
                playingTape = m_tape;
                m_audioSource.clip = m_tape.m_record;
                m_audioSource.Play();

            }
        }
    }


    public void playerInteractStopOrMissclick()
    {
        if (m_audioSource.isPlaying)
        {
            // Stop playing
            m_audioSource.Stop();
        }
        else
        {
            // Missclick
        }
    }


    // Update is called once per frame
    void Update()
    {

        tapeInTheTapePlayer.SetActive(m_audioSource.isPlaying);

        if (m_audioSource.isPlaying)
        {
            var probeRotationSpeed = 2;
            lProbe.transform.eulerAngles += lProbe.transform.forward * probeRotationSpeed;
            rProbe.transform.eulerAngles += rProbe.transform.forward * probeRotationSpeed;
        }

        
        if (!m_audioSource.isPlaying && playingTape != null)
        {
            playingTape.dropIt(gameObject);
            playingTape = null;
        }

    }
}
