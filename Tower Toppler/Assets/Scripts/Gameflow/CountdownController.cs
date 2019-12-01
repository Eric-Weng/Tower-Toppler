using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownController : MonoBehaviour
{
    #region Declarations

    public delegate void EventHandler();

    #endregion

    #region Members

    #pragma warning disable 0649
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float timerTime;

    private float timer;
    private bool canCount = false;
    private bool doOnce = false;
    private event EventHandler onTimeOut = null;
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
            if (onTimeOut != null)
            {
                onTimeOut();
            }
        }
    }

    #endregion

    #region Public Methods

    public void ResetTimer()
    {
        timer = timerTime;
        canCount = false;
        doOnce = false;
    }

    public void StartTimer()
    {
        canCount = true;
    }

    public void RegisterCallback(EventHandler handler)
    {
        onTimeOut += handler;
    }

    #endregion
}
