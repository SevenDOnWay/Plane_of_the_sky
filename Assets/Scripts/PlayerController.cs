using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour {
    [SerializeField] InputActionAsset inputActions;
    InputAction moveAction;

    [SerializeField] float horizontalSpeed = 5f;
    [SerializeField] float verticalSpeed = 3f;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxAngle = 40.0f;
    [SerializeField] float maxDragDistance;
    [SerializeField] float smoothMoveTime;
    [SerializeField] float smoothRotateTime;
    [SerializeField] float bounceEffect;

    Vector2 initPos;
    Vector2 currentPos;


    float currentAngle = 0f;
    float angularVelocity = 0f;
    float safeWidthScreen = 20f;
    float safeHeightScreen = 8.5f;

    Vector3 velocity;

    bool isDragging;

    private void OnEnable() {
        var playerMap = inputActions.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        moveAction.Enable();

        moveAction.started += OnStartHold;
        moveAction.performed += OnHoldPerformed;
        moveAction.canceled += OnHoldCanceled;
    }

    private void OnDisable() {
        moveAction.started -= OnStartHold;
        moveAction.performed -= OnHoldPerformed;
        moveAction.canceled -= OnHoldCanceled;
        moveAction.Disable();
    }

    void Update() {
        if (!isDragging) {
            currentAngle = Mathf.SmoothDampAngle(currentAngle, 0f, ref angularVelocity, 0.2f);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            return;
        }

        // Target movement delta (from drag)
        Vector2 delta = initPos - currentPos;
        delta = Vector2.ClampMagnitude(delta, maxDragDistance);

        Vector2 speedDelta = new Vector2(delta.x * horizontalSpeed, delta.y * verticalSpeed);

        // Desired target position
        Vector3 targetPosition = transform.position + (Vector3)(speedDelta * Time.deltaTime);

        // Clamp to safe zone
        targetPosition.x = Mathf.Clamp(targetPosition.x, -safeWidthScreen, safeWidthScreen);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -safeHeightScreen, safeHeightScreen);

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothMoveTime);

        // Rotate
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        currentAngle = angle;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    void OnStartHold(InputAction.CallbackContext ctx) {
        initPos = ctx.ReadValue<Vector2>();
        isDragging = true;
    }

    void OnHoldPerformed(InputAction.CallbackContext ctx) {
        if (isDragging)
            currentPos = ctx.ReadValue<Vector2>();
    }

    void OnHoldCanceled(InputAction.CallbackContext ctx) {
        isDragging = false;
        initPos = Vector2.zero;
        currentPos = Vector2.zero;
    }

}
