using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeadScreen : MonoBehaviour
{
    public Animator deadViewAnim;
    public GameObject deadView;
    public bool isDead = false;
    public CanvasGroupSwitcher CGS;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void onDead()
    {
        isDead = true;
        deadView.SetActive(true);
        deadViewAnim.Play("DeadWhiteFlash");
        StartCoroutine("mainMenuReturnCountdown");
    }
    IEnumerator mainMenuReturnCountdown()
    {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
