using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{

    public float clockSeconds = 0;
    private int nightHours = 6;
    public TextMeshProUGUI clockText;
    public int hour;
    private WinScreen m_winScreen;

    // Start is called before the first frame update
    void Start()
    {
        m_winScreen = GetComponent<WinScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clockSeconds < 60 * nightHours)
        {
            clockSeconds += Time.deltaTime * 2;
            hour = (int)(clockSeconds / 60);
            if (hour == 0)
            {
                clockText.text = "12 AM";
            }
            else
            {
                clockText.text = $"{hour} AM";
            }
        }
        else
        {
            if (!m_winScreen.isWin)
            {
                // game win
                m_winScreen.onWin();
            }
        }

    }
}
