using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuAnimationController : MonoBehaviour
{
    public static MainMenuAnimationController Instance;

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
    private const string FROM_EXTRAS_TO_TESTERS = "OnExtrasToTester";
    private const string FROM_TESTERS_TO_EXTRAS = "OnTesterToExtras";
    private const string FROM_CONTROLS_TO_HOME = "OnControlsToHome";
    private const string FROM_SETTINGS_TO_LANGUAGE = "OnSettingsToLanguage";
    private const string FROM_LANGUAGE_TO_SETTINGS = "OnLanguageToSettings";

    [Header("Selectable Buttons")]
    [SerializeField] public GameObject btnNewGame;
    [SerializeField] GameObject btnAudio;
    [SerializeField] GameObject dpdResolution;
    [SerializeField] GameObject sldGeneral;
    [SerializeField] GameObject btnPT;
    [SerializeField] GameObject btnBackControls;
    [SerializeField] GameObject btnBackExtra;
    [SerializeField] GameObject btnTesters;
    [SerializeField] GameObject btnDevs;

    GameObject goSelect;

    private Animator _an;

    private void Start()
    {
        _an = GetComponent<Animator>();
        Time.timeScale = 1;
    }

    public void GoToSettings()
    {
        _an.SetTrigger(GO_TO_SETTINGS);
        goSelect = btnAudio;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromSettingsToHome()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_HOME);
        goSelect = btnNewGame;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromSettingsToVideo()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_VIDEO);
        goSelect = dpdResolution;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromVideoToSettings()
    {
        _an.SetTrigger(FROM_VIDEO_TO_SETTINGS);
        goSelect = btnAudio;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromSettingsToAudio()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_AUDIO);
        goSelect = sldGeneral;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromAudioToSettings()
    {
        _an.SetTrigger(FROM_AUDIO_TO_SETTINGS);
        goSelect = btnAudio;
        Invoke("ButtonSelect", 1);
    }

    public void GoToExtras()
    {
        _an.SetTrigger(GO_TO_EXTRAS);
        goSelect = btnBackExtra;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromExtrasToHome()
    {
        _an.SetTrigger(FROM_EXTRAS_TO_HOME);
        goSelect = btnNewGame;
        Invoke("ButtonSelect", 1);
    }

    public void GoToGame()
    {
        _an.SetTrigger(GO_TO_GAME);
    }

    public void GoToControls()
    {
        _an.SetTrigger(GO_TO_CONTROLS);
        goSelect = btnBackControls;
        Invoke("ButtonSelect", 1);
    }
    public void GoFromControlsToHome()
    {
        _an.SetTrigger(FROM_CONTROLS_TO_HOME);
        goSelect = btnNewGame;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromSettingsToLanguage()
    {
        _an.SetTrigger(FROM_SETTINGS_TO_LANGUAGE);
        goSelect = btnPT;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromLanguageToSettings()
    {
        _an.SetTrigger(FROM_LANGUAGE_TO_SETTINGS);
        goSelect = btnAudio;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromExtrasToTester()
    {
        _an.SetTrigger(FROM_EXTRAS_TO_TESTERS);
        goSelect = btnDevs;
        Invoke("ButtonSelect", 1);
    }

    public void GoFromTesterToExtras()
    {
        _an.SetTrigger(FROM_TESTERS_TO_EXTRAS);
        goSelect = btnTesters;
        Invoke("ButtonSelect", 1);
    }

    public void ButtonSelect()
    {
        EventSystem.current.SetSelectedGameObject(goSelect);
    }
}
