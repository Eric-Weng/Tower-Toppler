using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region Members

    public Sound[] sounds;

    public static AudioManager instance;

    public float sliderPercentage;

    #endregion

    #region Unity Methods

    private void Awake()
    {

        if(instance == null) //Singleton
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }

        if(scene.name == "MainMenu")
        {
            Play("MainMenuMusic");
        }
        if(scene.name == "TowerEscape")
        {
            Play("ThemeMusic");
        }
        if(scene.name == "Credits")
        {
            Play("CreditsMusic");
        }
    }

    #endregion

    #region Public Methods

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void AdjustVolume(float newVolume)
    {
        sliderPercentage = newVolume / 1;

        foreach(Sound s in sounds)
        {
            s.source.volume = s.volume * sliderPercentage;
        }
    }

    public void PlayHover()
    {
        Play("ButtonHover");
    }

    #endregion

    //README: To use, in method where you want sound do  FindObjectOfType<AudioManager>().Play("NAME OF SOUND");
}
