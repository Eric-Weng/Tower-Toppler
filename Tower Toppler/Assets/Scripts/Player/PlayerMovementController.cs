using UnityEngine;
using UnityEngine.InputSystem;

#pragma warning disable 0618
public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private CharacterController characterController = null;
    private Controls controls = null;
    private Vector3 moveDirection = Vector3.zero;

    #endregion

    #region Unity Methods

    void Awake()
    {
        controls = new Controls();

        // Register input event for jumping
        controls.Player.Jump.performed += ctx => Jump();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Save current vertical speed for transforming
        float vertSpeed = moveDirection.y;

        // Apply lateral movement
        Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
        moveDirection = speed * transform.TransformDirection(input.x, 0.0f, input.y);

        // Rstore vertical speed
        moveDirection.y = vertSpeed;

        // Apply gravity in air
        if (!characterController.isGrounded)
        {
            moveDirection.y -= (gravity * Time.deltaTime);
        }

        // Move player
        characterController.Move(moveDirection * Time.deltaTime);
    }

    #endregion

    #region Public Methods

    public void Jump()
    {
        // perform jump when on ground
        if (characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
    }

    #endregion
}
