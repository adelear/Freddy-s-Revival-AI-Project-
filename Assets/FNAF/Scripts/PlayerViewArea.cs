using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyAI>().isFlashlightSensitive)
                other.GetComponent<EnemyAI>().inPlayerViewArea = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyAI>().isFlashlightSensitive)
                other.GetComponent<EnemyAI>().inPlayerViewArea = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
