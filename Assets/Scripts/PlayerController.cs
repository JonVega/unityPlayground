using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction movement;
    private InputAction jump;
    private InputAction looking;

    public Transform playerBody;
    public Camera cam;
    public float mouseSensitivity = 80f;
    private float xRotation = 0f;

    private void Awake() {
        //note: inputAction asset is not static or global
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable() {
        movement = playerInputActions.Player.Movement;
        movement.Enable(); //must call Enable() function or else input action won't work

        jump = playerInputActions.Player.Jump;
        jump.Enable();

        looking = playerInputActions.Player.MouseDelta;
        looking.Enable();
    }

    private void OnDisable() {
        movement.Disable();
        jump.Disable();
        looking.Disable();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked; //hides mouse cursor in game
        jump.performed += DoJump; //subscribe to event
    }

    private void FixedUpdate() //use physics engine to move and control player object
    {

    }

    private void Update() {
        Move();
        Look();
    }

    private void DoJump(InputAction.CallbackContext obj) {
        Debug.Log("Jump!");
    }

    private void Look() {
        float mouseX = looking.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        float mouseY = looking.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * looking.ReadValue<Vector2>().x);
    }

    private void Move() {
        Debug.Log("Movement Values: " + movement.ReadValue<Vector2>());
    }
}
