using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckExit : MonoBehaviour
{
    #region Unity Methods

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Detect collision with exit portal and move scene by portal name
        if (hit.collider.tag == "Exit")
        {
            string nextScene = hit.collider.name.Split('-')[1];
            SceneManager.LoadScene(nextScene);
        }
    }

    #endregion
}
