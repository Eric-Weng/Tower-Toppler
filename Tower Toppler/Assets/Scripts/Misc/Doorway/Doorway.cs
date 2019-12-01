using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    #region Fields

    private Animator anim = null;

    #endregion

    #region Unity Methods

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    #endregion

    #region Public Methods

    public void OpenDoorToRight()
    {
        anim.SetBool("openRight", true);
    }

    public void CloseDoorToRight()
    {
        anim.SetBool("openRight", false);
    }

    #endregion
}
