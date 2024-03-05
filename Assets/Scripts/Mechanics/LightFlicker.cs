using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light pointLight;
    private float minIntensity = 1.0f;
    private float maxIntensity = 50f;
    private float flickerSpeed = 10f; 
    
    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            pointLight.intensity = randomIntensity;

            yield return new WaitForSeconds(Random.Range(1f, flickerSpeed)); 
        }
    }
}
