using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
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

    public void RunEleavtor()
    {
        anim.SetBool("isRunning", true);
    }

    public void DisableElevator()
    {
        anim.SetBool("isRunning", false);
    }

    #endregion
}
