using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dpdRes;

    private Resolution[] resolutions;
    private List<Resolution> filteredRes;
    private Vector2[] ourResolutions = { new Vector2(1920, 1080), new Vector2(1600, 900), new Vector2(1366, 768), new Vector2(1280, 720) };

    private double currentRefreshRate;
    private int currentResolutionIndex = 0;
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredRes = new List<Resolution>();

        dpdRes.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;


        for (int i = 0; i < resolutions.Length; i++)
        {
            for (int j = 0; j < ourResolutions.Length; j++)
            {
                if (resolutions[i].width == (int)ourResolutions[j].x)
                {
                    if (resolutions[i].height == (int)ourResolutions[j].y)
                    {
                        if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
                        {
                            filteredRes.Add(resolutions[i]);
                        }
                    }
                }
            }
        }

            List<string> options = new List<string>();

        for(int i = 0; i < filteredRes.Count; i++)
        {
            string resOptions = filteredRes[i].width + "x" + filteredRes[i].height + " " + filteredRes[i].refreshRateRatio.value.ToString("@00") + " Hz";
            options.Add(resOptions);

            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        dpdRes.AddOptions(options);
        dpdRes.value = currentResolutionIndex;
        dpdRes.RefreshShownValue();

    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = filteredRes[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
