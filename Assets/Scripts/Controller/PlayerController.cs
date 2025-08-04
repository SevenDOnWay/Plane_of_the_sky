using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerController : MonoBehaviour {

    [SerializeField] InputActionAsset inputActions;
    InputAction moveAction;

    [Header("safe zone")]
    float safeWidthScreen;
    float safeHeightScreen;

    [Header("Player movement")]
    [SerializeField] float horizontalSpeed = 30f;
    [SerializeField] float verticalSpeed = 20f;

    [Header("Movement Config")]
    [SerializeField] float maxDragDistance = 100f;
    [SerializeField] float smoothMoveTime = 0.1f;
    [SerializeField] Vector2 spawnPoint = new Vector2(-2, -3);


    [Header("Player Rotation")]
    [SerializeField] float rotationFactor = 0.25f;
    [SerializeField] float maxRotationAngle = 45f;
    [SerializeField] float frequency = 1.5f;
    [SerializeField] float damping = 0.5f;

    [SerializeField] TextMeshProUGUI t_TravalDistance;
    public static int distanceTraveled;

    
    Vector2 initPos;
    Vector2 currentPos;


    float currentAngle = 0f;

    Vector3 velocity;

    bool isDragging;

    void Start() {
        Camera mainCamera = Camera.main;
        if ( mainCamera != null ) {
            // Get the height from camera's orthographic size
            safeHeightScreen = mainCamera.orthographicSize;
            // Calculate width based on aspect ratio
            safeWidthScreen = safeHeightScreen * mainCamera.aspect;
        }
        else { 
        Debug.LogError("Main Camera not found. Please ensure there is a camera tagged as 'MainCamera' in the scene.");
        }

        PlayAgainPannel.ClickHome += ResetPlayerRun;
    }

    void Awake() {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Enable();
        
        StateManager.Instance.OnGameEnded += Deactive;
    }

    void Update() {
        if ( StateManager.Instance != null && StateManager.Instance.isPlaying ) {
            HandleTouchInput();
            Vector2 moveDelta = CalculateMoveDelta();
            MovePlayer(moveDelta);
            RotatePlayer(moveDelta);
        }
    }

    void HandleTouchInput() {
        foreach ( var touch in Touch.activeTouches ) {
            switch ( touch.phase ) {
                case TouchPhase.Began:
                initPos = touch.screenPosition;
                isDragging = true;
                break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                currentPos = touch.screenPosition;
                break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                initPos = Vector2.zero;
                currentPos = Vector2.zero;
                isDragging = false;
                break;
            }
        }
    }

    #region Movement and Rotation Logic

    Vector2 CalculateMoveDelta() {
        Vector2 delta = currentPos - initPos;
        float dragDist = Mathf.Max(maxDragDistance, 0.001f);
        delta = Vector2.ClampMagnitude(delta, dragDist);
        Vector2 normalized = delta / maxDragDistance;
        return new Vector2(normalized.x * horizontalSpeed, normalized.y * verticalSpeed);
    }

    void MovePlayer(Vector2 moveDelta) {
        if ( float.IsNaN(moveDelta.x) || float.IsNaN(moveDelta.y) ) return;

        Vector3 targetPosition = transform.position + (Vector3) (moveDelta * Time.deltaTime);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -safeWidthScreen, safeWidthScreen);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -safeHeightScreen, safeHeightScreen);

        if ( float.IsNaN(targetPosition.x) || float.IsNaN(targetPosition.y) ) return;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothMoveTime);
    }

    void RotatePlayer(Vector2 moveDelta) {
        if ( isDragging ) {
            float angle = moveDelta.y / verticalSpeed * rotationFactor * maxDragDistance;
            currentAngle = Mathf.Clamp(angle, -maxRotationAngle, maxRotationAngle);
        }
        else {
            currentAngle = Spring(currentAngle, 0, frequency, damping, Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    //fre 1.5 , damp 0.5

    float Spring(float current, float target, float frequency, float damping, float deltaTime) {
        float omega = 2 * Mathf.PI * frequency;
        float x = omega * deltaTime;
        float exp = Mathf.Exp(-damping * x);

        float angle = (current - target) * exp * Mathf.Cos(x) + target;
        return angle;
    }

    #endregion    

    void Deactive() {
        //todo add effects, sfx, etc.

        distanceTraveled = int.Parse(t_TravalDistance.text);
        gameObject.SetActive(false);
    }

    void ResetPlayerRun() {
        gameObject.transform.position = spawnPoint;
        gameObject.transform.rotation = Quaternion.identity;
        t_TravalDistance.text = "0";
    }

    
}
