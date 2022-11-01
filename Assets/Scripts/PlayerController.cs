using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Action Map */
    //private PlayerInputActions playerInputActions;
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction movement;
    //private InputAction jump;
    private InputAction looking;

    public Transform playerBody;
    public Transform playerGroundCheck;
    public Camera cam;
    public LayerMask groundMask;
    public CharacterController controller;
    public float mouseSensitivity = 70f;
    private float xRotation = 0f;
    public float groundDistance = 0.4f; //radius of sphere going to check to see if on ground
    public float speed = 12f;
    public float gravity = -9.81f * 2; //towards 0 = slower fall, further negative = faster fall
    public float jumpHeight = 1.5f * 1;
    bool isGrounded;

    Vector3 velocity;

    private void Awake() {
        //note: inputAction asset is not static or global
        //playerInputActions = new PlayerInputActions();
        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void OnEnable() {
        player.FindAction("Jump").started += DoJump;
        player.Enable();
        movement = player.FindAction("Movement");
        looking = player.FindAction("Looking");
        
        // movement = playerInputActions.Player.Movement;
        // movement.Enable(); //must call Enable() function or else input action won't work

        // jump = playerInputActions.Player.Jump;
        // jump.Enable();

        // looking = playerInputActions.Player.MouseDelta;
        // looking.Enable();
    }

    private void OnDisable() {
        // movement.Disable();
        // jump.Disable();
        // looking.Disable();
        player.FindAction("Jump").started -= DoJump;
        player.Disable();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked; //hides mouse cursor in game
        //jump.performed += DoJump; //subscribe to event
    }

    private void FixedUpdate() //use physics engine
    {

    }

    private void Update() {
        Move();
        Look();
    }

    private void DoJump(InputAction.CallbackContext obj) {
        if(isGrounded) {
            //formula: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
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
        //Debug.Log("Movement Values: " + movement.ReadValue<Vector2>());
        isGrounded = Physics.CheckSphere(playerGroundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0.0f) {
            velocity.y = -2f;
        }
        Vector3 move = transform.right * movement.ReadValue<Vector2>().x + transform.forward * movement.ReadValue<Vector2>().y;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
