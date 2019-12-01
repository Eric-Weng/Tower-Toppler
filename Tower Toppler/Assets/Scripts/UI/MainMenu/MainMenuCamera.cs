using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    #region Fields

    public float maxRotation = 30f;
    public Vector3 initialRotation;

    private Controls controls = null;

    #endregion

    #region Unity Methods

    void Awake()
    {
        controls = new Controls();
    }

    void OnEnable()
    {
        controls.Menu.Enable();
    }

    void OnDisable()
    {
        controls.Menu.Disable();
    }

    void Update()
    {
        // Read mouse position
        Vector2 cursorPosition = controls.Menu.Pointer.ReadValue<Vector2>();

        // Calculate camera rotation
        float rotationX = Mathf.Clamp(cursorPosition.x * maxRotation / Screen.width, 0, maxRotation);
        float rotationY = Mathf.Clamp((Screen.height - cursorPosition.y) * maxRotation / Screen.height, 0, maxRotation);
        Vector3 rotation = new Vector3(rotationY, rotationX);

        // Update position
        transform.rotation = Quaternion.Euler(rotation + initialRotation);

    }

    #endregion
}
