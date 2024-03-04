using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameSession : MonoBehaviour
{

    public GameObject[] giftboxes;
    public GameObject[] giftboxSpawnPoints;

    public GameObject tape4;
    public GameObject tape5;
    public GameObject tape6;
    public GameObject tape7;

    public GameObject[] tapeSP4s;
    public GameObject[] tapeSP5s;
    public GameObject[] tapeSP6s;
    public GameObject[] tapeSP7s;

    // Start is called before the first frame update
    void Start()
    {

        if (giftboxes.Length != giftboxSpawnPoints.Length)
        {
            Debug.LogError("Giftboxes and GiftboxesSpawnPoints are not at the same length. Unable to random spawn giftboxes.");
        }
        else
        {
            for (int i = 0; i < giftboxes.Length; i++)
            {
                bool isSpawnDone = false;
                while (!isSpawnDone)
                {
                    int cidx = Random.Range(0, giftboxSpawnPoints.Length);
                    if (giftboxes[cidx] != null && giftboxSpawnPoints[cidx] != null)
                    {
                        giftboxes[cidx].transform.position = giftboxSpawnPoints[cidx].transform.position;

                        giftboxes[cidx] = null;
                        giftboxSpawnPoints[cidx] = null;

                        isSpawnDone = true;
                    }
                }

            }
        }

        GameObject tp4sp = tapeSP4s[Random.Range(0, tapeSP4s.Length)];
        tape4.transform.position = tp4sp.transform.position;
        GameObject tp5sp = tapeSP5s[Random.Range(0, tapeSP5s.Length)];
        tape5.transform.position = tp5sp.transform.position;
        GameObject tp6sp = tapeSP6s[Random.Range(0, tapeSP6s.Length)];
        tape6.transform.position = tp6sp.transform.position;
        GameObject tp7sp = tapeSP7s[Random.Range(0, tapeSP7s.Length)];
        tape7.transform.position = tp7sp.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
