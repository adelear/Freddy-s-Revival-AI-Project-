using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticPlateSpawner : ObjectSpawner
{
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> spawnedObjects = spawnObjects();
        foreach (var pplate in spawnedObjects)
        {
            pplate.GetComponent<Item>().inventory = inventory;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
