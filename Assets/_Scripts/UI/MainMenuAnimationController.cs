using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimationController : MonoBehaviour
{
    private const string GO_TO_SETTINGS = "OnSettings";
    private const string GO_TO_EXTRAS = "OnExtras";
    private const string GO_TO_GAME = "OnStartGame";
    private const string FROM_SETTINGS_TO_HOME = "OnSettingsToHome";
    private const string FROM_SETTINGS_TO_VIDEO = "OnSettingsToVideo";
    private const string FROM_VIDEO_TO_SETTINGS = "OnVideoToSettings";
    private const string FROM_SETTINGS_TO_AUDIO = "OnSettingsToAudio";
    private const string FROM_AUDIO_TO_SETTINGS = "OnAudioToSettings";
    private const string FROM_EXTRAS_TO_HOME = "OnExtrasToHome";

    private Animator _an;

    private void Start()
    {
        _an = GetComponent<Animator>();
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
}
