using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioManager asm; 
    [SerializeField] AudioClip LossSound;
    [SerializeField] AudioClip WinSound;
    [SerializeField] GameObject winPage;
    [SerializeField] GameObject lossPage; 
    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        GAME,
        PAUSE,
        DEFEAT,
        WIN
    }

    [SerializeField] private GameState currentState;
    public event System.Action<GameState> OnStateChanged; 
    public GameState CurrentState
    {
        get => currentState;
        set
        {
            if (currentState == value) return;
            currentState = value;
            OnStateChanged?.Invoke(currentState); 

            switch (currentState)
            {
                case GameState.GAME:
                    break;
                case GameState.PAUSE:
                    break; 
                case GameState.DEFEAT:
                    StartCoroutine(DelayedGameOver(2f)); 
                    break; 
                case GameState.WIN:
                    StartCoroutine(DelayedWin(1f)); 
                    break;
                default:
                    break; 
            }
        }
    }

    public event Action OnGameStateChanged;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
    }

    public IEnumerator DelayedGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameOver();
    }

    void GameOver()
    {
        lossPage.SetActive(true);
        AudioManager.Instance.PlayOneShot(LossSound, false);
    }

    void Win()
    {
        winPage.SetActive(true);
    }

    public IEnumerator DelayedWin(float delay)
    {
        AudioManager.Instance.PlayOneShot(WinSound, false); 
        yield return new WaitForSeconds(delay);
        Win(); 
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public void SwitchState(GameState newState)
    {
        Debug.Log("New state has been set to " + newState); 
        CurrentState = newState; 
        //OnGameStateChanged?.Invoke(); 
    }
}

