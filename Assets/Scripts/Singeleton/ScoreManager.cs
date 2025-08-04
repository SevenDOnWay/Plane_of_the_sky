using JetBrains.Annotations;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance { get; private set; }

    [Header("Score properties")]
    [SerializeField] float initSpeed = 10f;
    [SerializeField] float maxSpeed = 35f;
    [SerializeField] float accelerationRate = 0.25f;

    private float currentSpeed;


    bool isScoring;
    [SerializeField] GameObject scoreText;

    public float travelDistance;

    private void Start() {
        currentSpeed = initSpeed;
        travelDistance = 0f;


        StateManager.Instance.OnGameStarted += UpdateScore;
        StateManager.Instance.OnGameEnded += DisableScore;
        PlayAgainPannel.ClickHome += ResetScore;
    }

    void Awake() {
        if ( Instance != null && Instance != this ) {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Update() {
        if ( !isScoring ) return;

        if ( currentSpeed < maxSpeed ) {
            currentSpeed += accelerationRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }


        travelDistance += currentSpeed * Time.deltaTime;
    }

    void UpdateScore() {
        scoreText.SetActive(true);

        isScoring = true;
    }


    void DisableScore() {
        isScoring = false;
        scoreText.SetActive(false);
    }

    void ResetScore() {
        travelDistance = 0f;
        currentSpeed = initSpeed;
    }
}
