using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Members

    public bool GameIsPaused = false;
    Controls controls = null;

    [SerializeField] GameObject crosshair = null;

    private GameObject pauseMenuUI;
    private GameObject optionsMenuUI;
    private GameObject controlsMenuUI;
    private GameObject audioMenuUI;
    private GameObject player;
    private GameObject previousMenu;

    private Stack<GameObject> panelStack = new Stack<GameObject>();

    private PlayerCameraController playerCameraController;

    #endregion

    #region Unity Methods

    void Awake()
    {
        controls = new Controls();

        // Register input event for pausing game
        controls.Player.Pause.performed += ctx => Toggle();

        //Find Objects
        pauseMenuUI = GameObject.Find("PauseMenu");
        optionsMenuUI = GameObject.Find("OptionsMenu");
        audioMenuUI = GameObject.Find("AudioMenu");
        controlsMenuUI = GameObject.Find("ControlsMenu");
        player = GameObject.Find("Player");

        previousMenu = pauseMenuUI;
        panelStack.Push(previousMenu);
        playerCameraController = player.GetComponent<PlayerCameraController>();

        //Set Pause Menu Inactive
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        audioMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    #endregion

    #region Public Methods

    public void Toggle() //Toggles Game Pause
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume() //Disables PauseMenu and resumes game
    {
        while(panelStack.Count != 0)
        {
            panelStack.Peek().SetActive(false);
            panelStack.Pop();
        }
        previousMenu = pauseMenuUI;
        panelStack.Push(previousMenu);
        crosshair.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCameraController.enabled = true;
    }

    public void Pause() //Enables PauseMenu and pauses game
    {
        pauseMenuUI.SetActive(true);
        crosshair.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerCameraController.enabled = false;
    }

    public void OptionsButton()
    {
        previousMenu = optionsMenuUI;
        EnableMenu();  
    }

    public void AudioButton()
    {
        previousMenu = audioMenuUI;
        EnableMenu();
    }

    public void ControlsButton()
    {
        previousMenu = controlsMenuUI;
        EnableMenu();
    }

    public void BackButton()
    {
        panelStack.Peek().SetActive(false);
        panelStack.Pop();
        previousMenu = panelStack.Peek();
        previousMenu.SetActive(true);
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region Private Methods

    private void EnableMenu()
    {
        panelStack.Peek().gameObject.SetActive(false); //Turn off panel at bottom of stack
        panelStack.Push(previousMenu);  //Add current panel to top of stack
        panelStack.Peek().SetActive(true);  //Enable new panel  
    }

    #endregion
}
