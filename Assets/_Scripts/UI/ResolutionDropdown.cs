using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolutionDPD;

    private Resolution[] resolutions;
    //private List<Resolution> resolutionList;

    //private double currentRefreshRate;
    //private int currentResolutionIndex = 0;

    void Start()
    {
        resolutions = Screen.resolutions;
        this.GetAllResolutions(resolutions);

        this.WarmUpGame();

        //currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        //for (int i = 0; i < resolutions.Length; i++)
        //{
        //    if (RefreshRate.Equals(resolutions[i].refreshRateRatio, currentRefreshRate))
        //    {
        //        resolutionList.Add(resolutions[i]);
        //    }
        //}

        //List<string> options = new List<string>();

        //for (int i = 0; i < resolutionList.Count; i++)
        //{
        //    string resolutionOption = resolutionList[i].width + "x" + resolutionList[i].height + " " + resolutionList[i].refreshRateRatio.value + "Hz";
        //    options.Add(resolutionOption);
        //    if (resolutionList[i].width == Screen.width && resolutionList[i].height == Screen.height)
        //    {
        //        currentResolutionIndex = i;
        //    }
        //}

        //resolutionDPD.value = currentResolutionIndex;
        //resolutionDPD.RefreshShownValue();
    }

    private void OnEnable()
    {
        resolutionDPD.onValueChanged.AddListener(this.SetResolution);
    }

    private void OnDisable()
    {
        resolutionDPD.onValueChanged.RemoveListener(this.SetResolution);
    }

    private void WarmUpGame()
    {
        resolutionDPD.value = resolutions.Length - 1;
        this.SetResolution(resolutions.Length - 1);
        // resolutionDPD.SetValueWithoutNotify(resolution);
    }

    private void GetAllResolutions(Resolution[] resList)
    {
        List<string> resolutionList = new List<string>();

        foreach (Resolution resolution in resList)
        {
            resolutionList.Add($"{resolution.width}x{resolution.height} ({resolution.refreshRateRatio}Hz)");
        }

        resolutionDPD.ClearOptions(); // Limpa opções residuais no dropdown
        resolutionDPD.AddOptions(resolutionList);
    }

    public void SetResolution(int selectedIndex)
    {
        Resolution currestRes = resolutions[selectedIndex];

        Screen.SetResolution(currestRes.width, currestRes.height, Screen.fullScreenMode, currestRes.refreshRateRatio);

        // LocalStorage.SetResolution(selectedIndex);
    }

}
