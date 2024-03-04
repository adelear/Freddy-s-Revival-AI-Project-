using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    private Slider m_slider;
    public bool autoHide = true;

    // Start is called before the first frame update
    void Start()
    {
        m_slider = GetComponent<Slider>();
        gameObject.SetActive(autoHide);
    }

    public void resetProgressBar()
    {
        m_slider.value = 0;
        gameObject.SetActive(autoHide);
    }

    public void setProgressBarValue(float value)
    {
        gameObject.SetActive(true);
        m_slider.value = value / 100;
        m_slider.value = Mathf.Clamp(m_slider.value, 0, 1);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
