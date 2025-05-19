using TreeEditor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour {
    [SerializeField] InputActionAsset inputActions;
    InputAction moveAction;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxOffset;
    [SerializeField] float abc;

    Vector2 initPos;
    Vector2 currentPos;

    private void OnEnable() {
        var playerMap = inputActions.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        moveAction.Enable();

        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable() {
        moveAction.Disable();
    }

    void LateUpdate() {
        
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device) {
        var touch = Touchscreen.current;
        
        //uncomment when not using moblie

        // var mouse = Mouse.current;

        /*
        if (mouse != null) {
            if (mouse.leftButton.wasPressedThisFrame) {
                startPos = mouse.position.ReadValue();
                isDragging = true;
            }
            else if (mouse.leftButton.isPressed && isDragging) {
                currentPos = mouse.position.ReadValue();
            }
            else if (mouse.leftButton.wasReleasedThisFrame) {
                isDragging = false;
            }
        }
        */

        if (initPos == Vector2.zero && touch.primaryTouch.press.wasPressedThisFrame) initPos = touch.primaryTouch.position.ReadValue();
        else if (initPos != Vector2.zero && touch.primaryTouch.press.isPressed) currentPos = touch.primaryTouch.position.ReadValue();
        else if (touch != null && touch.primaryTouch.press.wasReleasedThisFrame) initPos = Vector2.zero;

    }

}
