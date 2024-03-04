using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RotateSkybox : MonoBehaviour
{

    public float rotationSpeed = 2;
    private Volume m_volume;
    //private HDRISky m_sky; 

    // Start is called before the first frame update
    void Start()
    {
        m_volume = GetComponent<Volume>();
        //m_volume.sharedProfile.TryGet<HDRISky>(out m_sky);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (m_sky != null)
        {
            Debug.Log(m_sky.rotation.value);
            var new_rot = m_sky.rotation.value + 2 * Time.deltaTime;
            if (new_rot >= 360)
                new_rot = 0;
            m_sky.rotation.Override(new_rot);
        }
        */ 

    }
}
