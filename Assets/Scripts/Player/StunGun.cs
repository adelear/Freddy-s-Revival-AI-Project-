using System.Collections;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeImg;
    EnemyController enemyController;
    private float fadeDuration = 0.5f; 
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
        StartCoroutine(FadeOut()); 
        enemyController.CurrentState = EnemyStates.Stun; 
        yield return new WaitForSeconds(30f);
        canStun = true;
        StartCoroutine(FadeIn());
        Debug.Log("Can Stun"); 
    }

    private IEnumerator FadeIn()
    {
        float timeElapsed = 0.0f;
        while (timeElapsed <= fadeDuration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            fadeImg.alpha = Mathf.Lerp(0.01f, 0.5f, timeElapsed / fadeDuration);
            yield return null;
        }

        fadeImg.alpha = 0.5f;
        yield return null;
    }

    private IEnumerator FadeOut()
    {
        float timeElapsed = 0.0f;

        fadeImg.alpha = 0.05f;

        while (timeElapsed <= fadeDuration)
        {

            fadeImg.alpha = Mathf.Lerp(0.5f, 0.01f, timeElapsed / fadeDuration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeImg.alpha = 0.01f;
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.GAME) return; 
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
        else enemyController = null;
    }
}
