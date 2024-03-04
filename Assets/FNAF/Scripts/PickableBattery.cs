using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBattery : MonoBehaviour
{

    public Flashlight flashlight;
    public float powerSeconds = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void pick()
    {
        flashlight.addBatterySeconds(powerSeconds);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
