using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    AudioManager audioManager;
    Slider thisSlider;
    

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        thisSlider = GetComponent<Slider>();
        thisSlider.value = audioManager.sliderPercentage;
        thisSlider.onValueChanged.AddListener(audioManager.AdjustVolume);
    }

}
