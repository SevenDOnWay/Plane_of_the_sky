using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerController : MonoBehaviour {
    
    [SerializeField] InputActionAsset inputActions;
    InputAction moveAction;
    
    [Header("Player movement")]
    [SerializeField] float horizontalSpeed = 30f;
    [SerializeField] float verticalSpeed = 20f;

    [Header("safe zone")]
    float safeWidthScreen = 20f;
    float safeHeightScreen = 8.5f;

    [Header("Player Rotation")]
    [SerializeField] float rotationFactor;
    [SerializeField] float frequency = 1.5f;
    [SerializeField] float damping = 0.5f;

    [SerializeField] float maxDragDistance;
    [SerializeField] float smoothMoveTime;
    [SerializeField] float smoothRotateTime;

    Vector2 initPos;
    Vector2 currentPos;


    float currentAngle = 0f;
    
    Vector3 velocity;

    bool isDragging;

    private void Awake() {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Enable();
    }

    private void OnEnable() {

        var playerMap = inputActions.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        moveAction.Enable();
    }

    private void OnDisable() {

        moveAction.Disable();
    }

    void Update() {
        foreach (var touch in Touch.activeTouches) {
            switch (touch.phase) {
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


        // Target movement delta (from drag)
        Vector2 delta = currentPos - initPos;
        delta = Vector2.ClampMagnitude(delta, maxDragDistance);
        Vector2 normalized = delta / maxDragDistance;

        Vector2 moveDelta = new Vector2(normalized.x * horizontalSpeed, normalized.y * verticalSpeed);

        // Desired target position
        Vector3 targetPosition = transform.position + (Vector3)(moveDelta * Time.deltaTime);

        // Clamp to safe zone
        targetPosition.x = Mathf.Clamp(targetPosition.x, -safeWidthScreen, safeWidthScreen);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -safeHeightScreen, safeHeightScreen);

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothMoveTime);

        //Rotate
        if (isDragging) {
            float angle = delta.y * rotationFactor;
            currentAngle = angle;
        }
        else {
            currentAngle = Spring(currentAngle, 0, frequency, damping, Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    #region
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param> 
    /// <param name="target"></param>
    /// <param name="frequency"></param>
    /// <param name="damping"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    #endregion

    float Spring(float current, float target, float frequency, float damping, float deltaTime) {
        float omega = 2 * Mathf.PI * frequency;
        float x = omega * deltaTime;
        float exp = Mathf.Exp(-damping * x);


        float angle = (current - target) * exp * (Mathf.Cos(x) + damping * Mathf.Sin(x)) + target;
        Debug.Log($"freq: {frequency}, damp: {damping}, dt: {deltaTime}, omega: {omega}, x: {x}, exp: {exp}, angle: {angle}");
        return angle;
    }
}
