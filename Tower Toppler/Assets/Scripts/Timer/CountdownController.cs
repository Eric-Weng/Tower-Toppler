using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    #region Members

    #pragma warning disable 0649
    [SerializeField] private Text uiText;
    [SerializeField] private float timerTime;

    private float timer;
    private bool canCount;
    private bool doOnce;

    public bool timerDone;
    #pragma warning restore 0649

    #endregion

    #region Unity Methods

    private void Start()
    {
        timer = timerTime;
    }

    private void Update()
    {
        if(timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            uiText.text = timer.ToString("F");
        }
        else if(timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;
            timerDone = true;
        }
    }

    #endregion

    #region Public Methods

    public void ResetTimer()
    {
        timer = timerTime;
        canCount = true;
        doOnce = false;
        timerDone = true;
    }

    #endregion
}
