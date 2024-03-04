using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FPSCounter : MonoBehaviour
{

    private TextMeshProUGUI m_fpsText;
    public int updateTime = 2;
    private int frameCount;
    private float time;
    public int frameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        m_fpsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        time += Time.deltaTime;

        if (time >= updateTime)
        {
            frameRate = Mathf.RoundToInt(frameCount / time);
            m_fpsText.text = frameRate.ToString() + " FPS";

            time -= updateTime;
            frameCount = 0;
            
        }


    }
}
