using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCam : MonoBehaviour
{
    private float raycastDistance = 5f; 
    void Update()
    {
        Vector3 direction = (transform.parent.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);

        RaycastHit hit;
        if (GameManager.Instance.GetGameState() == GameManager.GameState.DEFEAT && Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        { 
            Debug.Log("Object blocking camera!");
            MeshRenderer meshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>(); 
            if (meshRenderer != null) meshRenderer.enabled = false; 
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }
}
