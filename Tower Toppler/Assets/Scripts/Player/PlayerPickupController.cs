using System;
using UnityEngine;
using UnityEngine.InputSystem;

#pragma warning disable 0618
public class PlayerPickupController : MonoBehaviour
{

    #region Members

    public GameController gameController;
    public Transform PlayerHand;
    public Transform MaxReach;
    public Transform MinReach;
    public Camera PlayerCamera;
    public float maxPickupDistance;
    public float RotationSpeed;
    public float pushSpeed;

    private bool canInteract = false;
    private bool canRotate = false;
    private int m_InteractionMask = 0;
    private Controls controls = null;
    private GameObject m_HeldObject = null;
    private PlayerCameraController cameraController = null;

    #endregion

    #region Unity Methods

    void Awake()
    {
        controls = new Controls();

        // Register input event for interacting
        controls.Player.Interact.performed += ctx => Interact(ctx);

        // Register input event for toggling rotation
        controls.Player.Rotation.performed += ctx => ToggleObjectRotation();

        controls.Player.Push.performed += ctx => PushHeldObject(ctx);
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
        cameraController = GetComponent<PlayerCameraController>();

        // Find interactable mask layer
        m_InteractionMask = LayerMask.GetMask("Interactable");

        EnableInteraction();
    }

    void Update()
    {
        // If object is held rotate or push the object based on input
        if (m_HeldObject)
        {
            if (canRotate)
            {
                RotateHeldObject();
            }
        }
    }

    #endregion

    #region Public Methods

    public void EnableInteraction()
    {
        canInteract = true;
    }

    public void DisableInteraction()
    {
        canInteract = false;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (Pressed(context))
        {
            if (canInteract && !m_HeldObject)
            {
                HandleRaycast();
            }
        }
        else if (m_HeldObject)
        {
            Drop(m_HeldObject);
            m_HeldObject = null;
        }
    }

    #endregion

    #region Private Methods    

    private bool Pressed(InputAction.CallbackContext context)
    {
        return context.ReadValue<float>() == 1;
    }

    private void HandleRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, maxPickupDistance, m_InteractionMask))
        {
            m_HeldObject = hit.collider.gameObject;
            Pickup(m_HeldObject);
        }
    }

    private void Pickup(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;    
        obj.transform.SetParent(PlayerHand);
    }

    private void Drop(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        obj.transform.SetParent(null);
    }

    private bool RotateInputPressed()
    {
        // True if one of rotation buttons is held
        return Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)
            || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F)
            || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Z);
    }
    
    private void ToggleObjectRotation()
    {
        if (m_HeldObject)
        {
            canRotate = !canRotate;
            cameraController.ToggleCameraRotation();
        }
        else if (canRotate)
        {
            canRotate = false;
            cameraController.ToggleCameraRotation();
        }
    }

    private void RotateHeldObject()
    {
        // Recieve directional input
        Vector2 rotation2d = controls.Player.Camera.ReadValue<Vector2>();
        
        // Calculate rotation angles
        Quaternion rotationX = Quaternion.AngleAxis(RotationSpeed * rotation2d.y * Time.deltaTime, PlayerCamera.transform.right);
        Quaternion rotationZ = Quaternion.AngleAxis(RotationSpeed * -rotation2d.x * Time.deltaTime, PlayerCamera.transform.up);

        // Update object rotation
        m_HeldObject.transform.rotation = rotationX * rotationZ * m_HeldObject.transform.rotation;
    }

    private void PushHeldObject(InputAction.CallbackContext context)
    {
        // Ensure player is holding an object
        if (!m_HeldObject)
        {
            return;
        }

        // Read value from scroll wheel
        float scroll = Mathf.Sign(context.ReadValue<float>());

        // Select target direction based on scroll wheel value
        Transform target = (scroll > 0) ? MaxReach : MinReach;

        // Move towards target
        m_HeldObject.transform.position = Vector3.MoveTowards(m_HeldObject.transform.position, target.position, pushSpeed);
    }

    #endregion
}