using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class PlayerCameraController : MonoBehaviour
{

    #region Fields
    
    public float lookSensitivity;
    public float smoothing;
    public GameObject playerBody;
    public GameObject playerCamera;

    private float scaleFactor = 0.1f;
    private bool enable_rotation = true;
    private Controls controls = null;
    private Vector2 smoothedVelocity = Vector2.zero;
    private Vector2 currentLookingPos = Vector2.zero;

    #endregion


    #region Unity Methods

    void Awake()
    {
        controls = new Controls();
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
        Cursor.lockState = CursorLockMode.Locked; //Lock cursor in the middle of screen
        Cursor.visible = false; //Make cursor invisible
    }

    void Update()
    {
        if (enable_rotation)
        {
            RotateCamera(); //Rotate camera every frame
        }
    }

    #endregion

    #region Public Methods

    public void ToggleCameraRotation()
    {
        enable_rotation = !enable_rotation;
    }

    #endregion

    #region Private Methods

    private void RotateCamera()
    {
        Vector2 inputValues = scaleFactor * controls.Player.Camera.ReadValue<Vector2>(); //Makes Vector2 that gets mouse input
        //Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //Makes Vector2 that gets mouse input

        inputValues = Vector2.Scale(inputValues, new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing); //Creates smoothing in x-direction
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing); //Creates smoothing in y-direction

        currentLookingPos += smoothedVelocity; //Moves current looking position to new looking position

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -80, 80); //Clamps y so you can look up past clamp value
        playerCamera.transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right); 
        playerBody.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, playerBody.transform.up); //Rotates player when x-axis is traversed
    }

    #endregion
}
