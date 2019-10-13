using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour
{

    #region Members

    #pragma warning disable 0649
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
