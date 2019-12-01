using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RTBUIController : MonoBehaviour
{
    #region Members

    public CountdownController timer;
    public GameObject gameover;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI heightText;
    public GameObject endTurnCounter;
    public TextMeshProUGUI endTurnText;

    #endregion

    #region Public Methods

    public void ResetTimer()
    {
        timer.ResetTimer();
    }

    public void StartTimer()
    {
        timer.StartTimer();
    }
    public void RegisterTimerCallback(CountdownController.EventHandler handler)
    {
        timer.RegisterCallback(handler);
    }

    public void SetTurn(string turn)
    {
        turnText.text = turn;
    }

    public void SetHeight(string height)
    {
        heightText.text = height;
    }

    public void GameOver()
    {
        gameover.SetActive(true);
    }

    public void EndTurn()
    {
        endTurnCounter.SetActive(true);
    }

    public void SetEndTurnText(string text)
    {
        // Show end turn countdown timer
        if (!endTurnCounter.activeSelf)
        {
            endTurnCounter.SetActive(true);
        }

        // Update value
        endTurnText.text = (text != "0") ? text : "";
    }

    #endregion
}
