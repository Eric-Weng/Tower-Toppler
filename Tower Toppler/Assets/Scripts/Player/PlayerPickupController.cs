using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class PlayerPickupController : NetworkBehaviour
{

    #region Members
    
    public Transform PlayerHand;
    public Camera PlayerCamera;
    public float RotationSpeed;

    private int m_InteractionMask = 0;
    private NetworkIdentity m_HeldObject = null;

    #endregion

    #region Unity Methods

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        m_InteractionMask = LayerMask.GetMask("Interactable");
    }

    void Update()
    {
        // Ensure local player only modifies local object
        if (!isLocalPlayer)
        {
            return;
        }

        // If input pressed interact with object in front of player
        if (InteractInputPressed())
        {
            Interact();
        }

        // If rotation input and an object is held rotate the object
        if (RotateInputPressed() && m_HeldObject)
        {
            RotateHeldObject();
        }
    }

    #endregion

    #region Private Methods

    private bool PickupInputPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool DropInputPressed()
    {
        return Input.GetMouseButtonUp(0);
    }

    private bool InteractInputPressed()
    {
        return PickupInputPressed() || DropInputPressed();
    }

    private void Interact()
    {
        if (PickupInputPressed())
        {
            HandleRaycast();
        }
        else if (m_HeldObject)
        {
            Drop(m_HeldObject);
            m_HeldObject = null;
        }
    }

    private void HandleRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, Mathf.Infinity, m_InteractionMask))
        {
            NetworkIdentity objNetId = hit.collider.GetComponent<NetworkIdentity>();

            if (objNetId.hasAuthority)
            {
                m_HeldObject = objNetId;
                Pickup(m_HeldObject);
            }
        }
    }

    private void Pickup(NetworkIdentity obj)
    {
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        obj.transform.SetParent(PlayerHand.transform);
    }

    private void Drop(NetworkIdentity obj)
    {
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        obj.transform.SetParent(null);
    }

    private bool RotateInputPressed()
    {
        return Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)
            || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Z);
    }

    private void RotateHeldObject()
    {
        float zAxis = (Convert.ToSingle(Input.GetKey(KeyCode.Q)) - Convert.ToSingle(Input.GetKey(KeyCode.E)));
        float xAxis = (Convert.ToSingle(Input.GetKey(KeyCode.C)) - Convert.ToSingle(Input.GetKey(KeyCode.Z)));

        Quaternion rotationZ = Quaternion.AngleAxis(RotationSpeed * zAxis * Time.deltaTime, PlayerCamera.transform.forward);
        Quaternion rotationX = Quaternion.AngleAxis(RotationSpeed * xAxis * Time.deltaTime, PlayerCamera.transform.right);

        m_HeldObject.transform.rotation = rotationZ * rotationX * m_HeldObject.transform.rotation;
    }

    #endregion
}