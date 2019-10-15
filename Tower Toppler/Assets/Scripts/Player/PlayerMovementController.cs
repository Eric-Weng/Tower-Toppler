using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class PlayerMovementController : NetworkBehaviour
{

    #region Fields
    
    public float speed;

    private Rigidbody m_RigidBody;

    #endregion


    #region Unity Methods

    private void Start()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

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
        
        Vector3 force = transform.TransformDirection(new Vector3(xAxis, yAxis, zAxis) * speed * modifier);
        m_RigidBody.AddForce(force);
    }

    #endregion
}
