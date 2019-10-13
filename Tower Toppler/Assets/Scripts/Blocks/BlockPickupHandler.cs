using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPickupHandler : MonoBehaviour, IInteractable
{

    #region Members

    private bool m_IsHeld;
    private Rigidbody m_RigidBody;

    #endregion

    #region Unity Methods

    void Start()
    {
        m_IsHeld = false;
        m_RigidBody = GetComponent<Rigidbody>();
    }

    #endregion

    #region IInteractable Methods

    public void OnInteract()
    {
        if (!m_IsHeld)
        {
            Pickup();
        }
        else
        {
            Drop();
        }
    }

    #endregion

    #region Private Methods

    void Pickup()
    {
        m_RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        m_IsHeld = true;
    }

    void Drop()
    {
        m_RigidBody.constraints = RigidbodyConstraints.None;
        m_IsHeld = false;
    }

    #endregion
}
