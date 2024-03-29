﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFallHandler : MonoBehaviour
{

    #region Declarations

    public delegate void EventHandler();

    #endregion

    #region Members

    private event EventHandler OnTowerFall;

    #endregion

    #region Unity Methods

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Block") && OnTowerFall != null)
        {
            OnTowerFall();
        }
    }

    #endregion

    #region Public Methods

    public void RegisterTowerFallHandler(EventHandler handler)
    {
        OnTowerFall += handler;
    }

    #endregion
}
