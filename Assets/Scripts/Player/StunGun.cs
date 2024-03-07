using System.Collections;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    EnemyController enemyController;

    float hitDistance = 7f;
    bool canStun = true; 
    void StunEnemy()
    {
        if (enemyController != null && !enemyController.isStunned && !enemyController.isDead && canStun) StartCoroutine(BeginStun());   
    }

    IEnumerator BeginStun()
    {
        Debug.Log("Stunning Enemy");  
        canStun = false; 
        enemyController.CurrentState = EnemyStates.Stun; 
        yield return new WaitForSeconds(30f);
        canStun = true;
        Debug.Log("Can Stun"); 
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, hitDistance))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.gameObject.GetComponent<EnemyController>() != null)
            {
                enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                if (Input.GetMouseButtonDown(1)) 
                {
                    StunEnemy();
                }
            }
        }
        else
        {
            enemyController = null;
        }
    }
}
