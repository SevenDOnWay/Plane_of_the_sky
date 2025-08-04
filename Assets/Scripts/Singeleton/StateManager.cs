using System;
using Unity.Mathematics;
using UnityEngine;

public class StateManager : MonoBehaviour {
    public static StateManager Instance { get; private set; }

    public event Action OnGameStarted;
    public event Action OnGameEnded;
    public event Action StopPauseGame; // for now idk

    public bool isPlaying { get; private set; }
    public bool isPausing { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame() {
        isPlaying = true;
        // You can also show/hide UI, reset score, etc.

        OnGameStarted?.Invoke();
    }

    public void EndGame() {
        isPlaying = false;

        OnGameEnded?.Invoke();

        Debug.Log("Game Over!");
        // Show Game Over screen, stop movement, etc.
    }

    public void PauseGame() {

        isPlaying = false;
        isPausing = true;
    }

    public void ResumeGame() {
        isPlaying = true;
        isPausing = false;

        StopPauseGame?.Invoke();
    }


}
