using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    #region Fields

    public UnityEvent pressurePlatePressed;
    public UnityEvent pressurePlateReleased;

    private Animator anim = null;

    #endregion

    #region Unity Methods

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if pressure plate is pressed
        if (ValidTrigger(other))
        {
            // Play pressing animation
            anim.SetBool("isPressed", true);
            
            // Perform functionality of pressure plate press
            if (pressurePlatePressed != null)
            {
                pressurePlatePressed.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if pressure plate is released
        if (ValidTrigger(other))
        {
            // Play release animation
            anim.SetBool("isPressed", false);

            // Perform functionality of pressure plate release
            if (pressurePlateReleased != null)
            {
                pressurePlateReleased.Invoke();
            }
        }
    }

    #endregion

    #region Private Methods

    private bool ValidTrigger(Collider other)
    {
        // Only allow the player and blocks to press the pressure plate
        return other.tag == "Player" || other.tag == "Block";
    }

    #endregion
}
