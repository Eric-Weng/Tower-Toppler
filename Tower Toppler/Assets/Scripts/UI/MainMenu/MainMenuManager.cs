using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    #region Members

    private GameObject mainMenuPanel;
    private GameObject multiplayerPanel;
    private GameObject optionsPanel;
    private GameObject controlsPanel;
    private GameObject currentPanel;
    private GameObject audioPanel;
    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private Animator animator = null;

    #endregion

    #region Unity Methods

    private void Start() //Find and assign all panels and add Main Menu to the Stack
    {
        mainMenuPanel = GameObject.Find("MainMenuPanel");
        multiplayerPanel = GameObject.Find("MultiMenuPanel");
        optionsPanel = GameObject.Find("OptionsMenuPanel");
        controlsPanel = GameObject.Find("ControlsPanel");
        audioPanel = GameObject.Find("AudioPanel");
        panelStack.Push(mainMenuPanel);
    }
    

    #endregion

    #region Public Methods

    public void PlayGame()
    {
        SceneManager.LoadScene("TowerEscape");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenMultiPanel()
    {
        currentPanel = multiplayerPanel;
        SlidePanel();
    }

    public void OpenOptionsPanel()
    {
        currentPanel = optionsPanel;
        SlidePanel();
    }

    public void OpenControlsPanel()
    {
        currentPanel = controlsPanel;
        SlidePanel();
    }

    public void OpenAudioPanel()
    {
        currentPanel = audioPanel;
        SlidePanel();
    }

    public void BackButton()
    {
        if(animator != null)
        {
            animator.SetTrigger("Close"); //Play Close animation for current panel
        }

        panelStack.Pop(); //Remove top panel off stack
        currentPanel = panelStack.Peek(); //Make panel under new current panel
        currentPanel.gameObject.SetActive(true); //Turn on panel
        animator = currentPanel.GetComponent<Animator>();
    }

    #endregion

    #region Private Methods

    private void SlidePanel()
    {
        panelStack.Peek().gameObject.SetActive(false); //Turn off panel at bottom of stack
        panelStack.Push(currentPanel);  //Add current panel to top of stack
        animator = currentPanel.GetComponent<Animator>(); //Get current panels animator
        {
            if(animator != null)
            {
                animator.SetTrigger("Open"); //Play open animation for current panel
            }
        }
    }

    #endregion
}
