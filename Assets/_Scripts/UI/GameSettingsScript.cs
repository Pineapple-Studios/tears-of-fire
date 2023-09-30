using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameSettingsScript : MonoBehaviour
{
    [SerializeField] Toggle tgl_FullScreen;
    [SerializeField] TMP_Dropdown sel_Resolutions;
    [SerializeField] Slider sld_AudioVolume;
    [SerializeField] AudioMixer amix_Master;

    private Resolution[] supportedResolutions;

    private void Start()
    {
        tgl_FullScreen.isOn = Screen.fullScreen;
        supportedResolutions = Screen.resolutions;
        this.GetAllResolutions(supportedResolutions);

        this.WarmUpGame();
    }

    private void OnEnable()
    {
        tgl_FullScreen.onValueChanged.AddListener(this.ToggleFullScreen);
        sel_Resolutions.onValueChanged.AddListener(this.SetResolution);
        sld_AudioVolume.onValueChanged.AddListener(this.OnChangeVolume);
    }

    private void OnDisable()
    {
        tgl_FullScreen.onValueChanged.RemoveListener(this.ToggleFullScreen);
        sel_Resolutions.onValueChanged.RemoveListener(this.SetResolution);
        sld_AudioVolume.onValueChanged.RemoveListener(this.OnChangeVolume);
    }

    private void WarmUpGame()
    {
       // // Volume
       // //float volume = LocalStorage.GetVolume(0.6f);
       // this.OnChangeVolume(volume);
       // sld_AudioVolume.SetValueWithoutNotify(volume);

       // // Resolution
       //// int resolution = LocalStorage.GetResolution(supportedResolutions.Length - 1);
       // this.SetResolution(resolution);
       // sel_Resolutions.SetValueWithoutNotify(resolution);

       // // FullScreen
       //// bool isFullScreen = LocalStorage.GetFullScreen(false);
       // this.ToggleFullScreen(isFullScreen);
       // tgl_FullScreen.SetIsOnWithoutNotify(isFullScreen);
    }

    private void GetAllResolutions(Resolution[] resList)
    {
        List<string> resolutionList = new List<string>();
        
        foreach (Resolution resolution in resList)
        {
            resolutionList.Add($"{resolution.width}x{resolution.height} ({resolution.refreshRateRatio}Hz)");
        }

        sel_Resolutions.ClearOptions(); // Limpa opções residuais no dropdown
        sel_Resolutions.AddOptions(resolutionList);
    }

    public void ToggleFullScreen(bool newState)
    {
        Screen.fullScreen = newState;

        //LocalStorage.SetFullScreen(newState);
    }

    public void SetResolution(int selectedIndex)
    {
        Resolution currestRes = supportedResolutions[selectedIndex];

        Screen.SetResolution(currestRes.width, currestRes.height, Screen.fullScreenMode, currestRes.refreshRateRatio);

        //LocalStorage.SetResolution(selectedIndex);
    }

    public void OnChangeVolume(float value)
    {
        float calculatedVolume = Mathf.Log10(value) * 20;
        amix_Master.SetFloat("MasterVolume", calculatedVolume);

        //LocalStorage.SetVolume(value);
    }
}
