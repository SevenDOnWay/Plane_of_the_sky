using System;
using Unity.Mathematics;
using UnityEngine;

public class StateController : MonoBehaviour {
    public static StateController Instance { get; private set; }

    public event Action OnGameEnded;

    public bool isPlaying { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartGame() {
        isPlaying = true;
        // You can also show/hide UI, reset score, etc.
    }

    public void EndGame() {
        isPlaying = false;

        OnGameEnded?.Invoke();

        Debug.Log("Game Over!");
        // Show Game Over screen, stop movement, etc.
    }
}
