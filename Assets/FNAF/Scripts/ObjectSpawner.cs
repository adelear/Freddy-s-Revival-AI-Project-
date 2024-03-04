using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 spawnRotation;
    public int spawnCount = 1;
    public bool scatterAround = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    protected List<GameObject> spawnObjects()
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        for (int i = 0; i < spawnCount; i++)
        {

            var spawnPosition = transform.position + (scatterAround ? new Vector3(Random.Range(0f, 2f), 0, Random.Range(0f, 2f)) : Vector3.zero);

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(spawnRotation), transform);
            spawnedObjects.Add(spawnedObject);
        }

        return spawnedObjects;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
