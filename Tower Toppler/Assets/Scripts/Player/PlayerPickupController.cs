using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class PlayerPickupController : NetworkBehaviour
{

    #region Members

    #pragma warning disable 0649
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private GameObject m_PlayerHand;
    #pragma warning disable 0649

    private Transform m_HeldObject;

    #endregion

    #region Unity Methods

    void Start()
    {
        m_HeldObject = null;
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(m_PlayerHand.transform.position, m_PlayerHand.transform.TransformDirection(Vector3.forward), out hit))
            {
                OnRayHit(hit);
            }
        }
        else if (Input.GetMouseButtonUp(0) && m_HeldObject != null)
        {
            m_HeldObject.GetComponent<IInteractable>().OnInteract();
            Drop();
        }
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            return;
        }

        if (m_HeldObject)
        {
            if (Input.GetKey(KeyCode.E))
            {
                m_HeldObject.transform.Rotate(new Vector3(0, 1, 0) * m_RotationSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                m_HeldObject.transform.Rotate(new Vector3(0, -1, 0) * m_RotationSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                m_HeldObject.transform.Rotate(new Vector3(10, 0, 0) * m_RotationSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                m_HeldObject.transform.Rotate(new Vector3(-10, 0, 0) * m_RotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    #endregion

    #region Private Methods

    void OnRayHit(RaycastHit hit)
    {
        IInteractable interactable;

        if ((interactable = hit.collider.GetComponent<IInteractable>()) != null && m_HeldObject == null)
        {
            interactable.OnInteract();
            Pickup(hit.collider.GetComponent<Transform>());
        }
    }

    void Pickup(Transform interactable)
    {
        interactable.SetParent(m_PlayerHand.transform);
        m_HeldObject = interactable;
    }

     void Drop()
    {
        m_HeldObject.SetParent(null);
        m_HeldObject = null;
    }

    #endregion
}
