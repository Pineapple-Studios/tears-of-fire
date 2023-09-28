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
    private List<Resolution> resolutionList;

    private float currentRefreshRate;
    private float currentResolutionIndex = 0;
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionList = new List<Resolution>();

        resolutionDPD.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                resolutionList.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < resolutionList.Count; i++)
        {
            string resolutionOption = resolutionList[i].width + "x" + resolutionList[i].height + " " + resolutionList[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            if (resolutionList[i].width == Screen.width && resolutionList[i].height == Screen.height)
            {
                currentResolutionIndex = i;

            }
        }

        resolutionDPD.AddOptions(options);
        resolutionDPD.value = (int)currentResolutionIndex;
        resolutionDPD.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

}
