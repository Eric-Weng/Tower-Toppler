using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
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

    public void ExtendPiston()
    {
        anim.SetBool("isExtended", true);
    }

    public void RetractPiston()
    {
        anim.SetBool("isExtended", false);
    }

    #endregion
}
