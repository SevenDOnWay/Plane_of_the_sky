using UnityEngine;

public class StateController : MonoBehaviour {
    public static StateController Instance { get; private set; }

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
        // Show Game Over screen, stop movement, etc.
    }
}
