using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Video;

public class CutsceneLocale : MonoBehaviour
{
    private const string LOC_ENGLISH_ID = "English (en)";

    [SerializeField] VideoClip cutsceneEn;
    [SerializeField] VideoClip cutscenePt;

    VideoPlayer vp;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        LocaleCutscene();
    }

    void LocaleCutscene()
    {
        string loc = LocalizationSettings.SelectedLocale.name;

        if (loc == LOC_ENGLISH_ID && cutsceneEn != null)
        {
            vp.clip = cutsceneEn;
        }
        else
        {
            vp.clip = cutscenePt;
        }
    }
}
