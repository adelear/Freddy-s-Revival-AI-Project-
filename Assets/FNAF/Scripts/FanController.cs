using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{

    public Animator fanAnimator;
    public float fMaxSpeed = 5;
    public float fMinSpeed = 0;
    public bool isFanOn = true;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void triggerOnOff()
    {

        isFanOn = !isFanOn;
    }


    // Update is called once per frame
    void Update()
    {

        if (isFanOn)
            fanAnimator.speed = fMaxSpeed;
        else
            fanAnimator.speed = fMinSpeed;


    }
}
