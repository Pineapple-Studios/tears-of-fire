using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimationController : MonoBehaviour
{
    private const string GO_TO_SETTINGS = "OnSettings";
    private const string GO_TO_EXTRAS = "OnExtras";
    private const string GO_TO_CONTROLS = "OnControls";
    private const string GO_TO_GAME = "OnStartGame";
    private const string FROM_SETTINGS_TO_HOME = "OnSettingsToHome";
    private const string FROM_SETTINGS_TO_VIDEO = "OnSettingsToVideo";
    private const string FROM_VIDEO_TO_SETTINGS = "OnVideoToSettings";
    private const string FROM_SETTINGS_TO_AUDIO = "OnSettingsToAudio";
    private const string FROM_AUDIO_TO_SETTINGS = "OnAudioToSettings";
    private const string FROM_EXTRAS_TO_HOME = "OnExtrasToHome";
    private const string FROM_CONTROLS_TO_HOME = "OnControlsToHome";
    private const string FROM_SETTINGS_TO_LANGUAGE = "OnSettingsToLanguage";
    private const string FROM_LANGUAGE_TO_SETTINGS = "OnLanguageToSettings";

    private Animator _an;

    private void Start()
    {
        _an = GetComponent<Animator>();
        Time.timeScale = 1;
    }

    public void GoToSettings()
    {
        _an.SetTrigger(GO_TO_SETTINGS);
    }

    public void GoFromSettingsToHome()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_HOME);
    }

    public void GoFromSettingsToVideo()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_VIDEO);
    }

    public void GoFromVideoToSettings()
    {
        _an.SetTrigger(FROM_VIDEO_TO_SETTINGS);
    }

    public void GoFromSettingsToAudio()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_AUDIO);
    }

    public void GoFromAudioToSettings()
    {
        _an.SetTrigger(FROM_AUDIO_TO_SETTINGS);
    }

    public void GoToExtras()
    {
        _an.SetTrigger(GO_TO_EXTRAS);
    }

    public void GoFromExtrasToHome()
    {
        _an.SetTrigger(FROM_EXTRAS_TO_HOME);
    }

    public void GoToGame()
    {
        _an.SetTrigger(GO_TO_GAME);
    }
    public void GoToControls()
    {
        _an.SetTrigger(GO_TO_CONTROLS);
    }
    public void GoFromControlsToHome()
    {
        _an.SetTrigger(FROM_CONTROLS_TO_HOME);
    }

    public void GoFromSettingsToLanguage()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_LANGUAGE);
    }

    public void GoFromLanguageToSettings()
    {
        _an.SetTrigger(FROM_LANGUAGE_TO_SETTINGS);
    }
}
