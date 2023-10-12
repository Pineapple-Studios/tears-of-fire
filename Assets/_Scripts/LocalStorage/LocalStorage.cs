using UnityEngine;

public class LocalStorage
{
    const string GENERAL_VOLUME = "@TOF_GENERAL_VOLUME";
    const string MUSIC_VOLUME = "@TOF_MUSIC_VOLUME";
    const string SFX_VOLUME = "@TOF_SFX_VOLUME";
    //const string VOICES_VOLUME = "@TOF_VOICES_VOLUME";
    const string RESOLUTION = "@TOF_RESOLUTION";
    const string FULL_SCREEN = "@TOF_FULL_SCREEN";

    #region GeneralVolume
    public static void SetGeneralVolume(float currentValue)
    {
        PlayerPrefs.SetFloat(GENERAL_VOLUME, currentValue);
    }

    public static float GetGeneralVolume(float defaultValue)
    {
        return PlayerPrefs.GetFloat(GENERAL_VOLUME, defaultValue);
    }
    #endregion

    #region MusicVolume
    public static void SetMusicVolume(float currentValue)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, currentValue);
    }

    public static float GetMusicVolume(float defaultValue)
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME, defaultValue);
    }
    #endregion

    #region SFXVolume
    public static void SetSFXVolume(float currentValue)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, currentValue);
    }

    public static float GetSFXVolume(float defaultValue)
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME, defaultValue);
    }
    #endregion

    /*#region VoicesVolume
    public static void SetVoicesVolume(float currentValue)
    {
        PlayerPrefs.SetFloat(VOICES_VOLUME, currentValue);
    }

    public static float GetVoicesVolume(float defaultValue)
    {
        return PlayerPrefs.GetFloat(VOICES_VOLUME, defaultValue);
    }
    #endregion*/

    #region Resolution
    public static void SetResolution(int currentIndex)
    {
        PlayerPrefs.SetInt(RESOLUTION, currentIndex);
    }

    public static int GetResolution(int defaultValue)
    {
        return PlayerPrefs.GetInt(RESOLUTION, defaultValue);
    }
    #endregion

    /*#region FullScreen
    public static void SetFullScreen(bool currentValue)
    {
        PlayerPrefsX.SetBool(FULL_SCREEN, currentValue);
    }

    public static bool GetFullScreen(bool defaultValue)
    {
        return PlayerPrefsX.GetBool(FULL_SCREEN, defaultValue);
    }
    #endregion*/
}