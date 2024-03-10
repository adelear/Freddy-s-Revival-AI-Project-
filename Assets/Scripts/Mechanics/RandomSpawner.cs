using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] GameObject randomObject;
    [SerializeField] Transform[] randomPoints; 
    private int randomIndex;

    private void Start()
    {
        randomIndex = Random.Range(0, randomPoints.Length);
        Instantiate(randomObject, randomPoints[randomIndex].position, randomPoints[randomIndex].rotation);
    } 
}
