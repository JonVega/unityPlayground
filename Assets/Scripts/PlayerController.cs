using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction movement;


    private void Awake() {
        //note: inputAction asset is not static or global
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable() {
        movement = playerInputActions.Player.Movement;
        movement.Enable(); //must call this function or input action won't work

        playerInputActions.Player.Jump.performed += DoJump;
        playerInputActions.Player.Jump.Enable();
    }

    private void OnDisable() {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
    }

    private void DoJump(InputAction.CallbackContext obj) {
        Debug.Log("Jump!");
    }

    private void FixedUpdate() //use physics engine to move and control player object
    {
        Debug.Log("Movement Values: " + movement.ReadValue<Vector2>());
    }
}
