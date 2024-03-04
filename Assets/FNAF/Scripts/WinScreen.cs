using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public Animator winViewAnim;
    public GameObject winView;
    public CanvasGroupSwitcher CGS;
    public AudioSource winAudio;
    private AudioSource gameBGSounds;
    public bool isWin = false;

    // Start is called before the first frame update
    void Start()
    {
        gameBGSounds = GetComponent<AudioSource>();
    }

    public void destroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
            Destroy(enemy);
    }

    public void onWin()
    {
        isWin = true;
        destroyAllEnemies();
        CGS.setUI(UserInterfaces.None);
        winView.SetActive(true);
        winViewAnim.Play("WinView");
        gameBGSounds.enabled = false;
        winAudio.enabled = true;
        winAudio.Play();
        StartCoroutine("mainMenuReturnCountdown");
    }

    IEnumerator mainMenuReturnCountdown()
    {
        yield return new WaitForSeconds(14);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
