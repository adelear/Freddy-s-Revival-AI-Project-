using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light pointLight;
    public float minIntensity = 1.0f;
    public float maxIntensity = 50f;
    private float flickerSpeed = 10f;
    public int minFlickerCount = 1;
    public int maxFlickerCount = 10;
    private float fadeDuration = 0.1f; 

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            int flickerCount = Random.Range(minFlickerCount, maxFlickerCount); 
            for (int i = 0; i < flickerCount; i++)
            {
                // Flickeringgg off
                float fadeStep = (pointLight.intensity - 0f) / (fadeDuration / Time.deltaTime);
                while (pointLight.intensity > 0f)
                {
                    pointLight.intensity -= fadeStep;
                    yield return null;
                }

                // back onn
                float randomIntensity = Random.Range(minIntensity, maxIntensity);
                pointLight.intensity = randomIntensity;


                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }

            yield return new WaitForSeconds(Random.Range(1f, flickerSpeed));
        }
    }
}
