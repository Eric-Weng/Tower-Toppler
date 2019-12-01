using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    #region Unity Methods

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    #region Public Methods

    public void EndCredits()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
