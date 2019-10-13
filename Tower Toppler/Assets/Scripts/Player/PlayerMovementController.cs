using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    #region Fields

    #pragma warning disable 0649 
    [SerializeField] private float speed;
    [SerializeField] GameObject playerBody;
    #pragma warning restore 0649

    private Rigidbody m_RigidBody;

    #endregion


    #region Unity Methods

    private void Start()
    {
        m_RigidBody = playerBody.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #endregion


    #region Private Methods

    private void Move()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        float yAxis = 0f;
        if (Input.GetKey(KeyCode.Space) ^ Input.GetKey(KeyCode.LeftShift))
        {
            yAxis = Input.GetKey(KeyCode.Space) ? 1f : -1f;
        }

        float modifier = Input.GetKey(KeyCode.LeftControl) ? 0.5f : 1f;
        
        Vector3 force = playerBody.transform.TransformDirection(new Vector3(xAxis, yAxis, zAxis) * speed * modifier);
        m_RigidBody.AddForce(force);
    }

    #endregion
}
