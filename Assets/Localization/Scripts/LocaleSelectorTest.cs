using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocaleSelectorTest : MonoBehaviour
{
    [SerializeField] GameObject loading;

    private bool _active = false;
    private bool _defined = false;

    private void Update()
    {
        if (_defined == true && SceneManager.GetActiveScene().name == "SelectScreen")
        { 
            SceneManager.LoadScene("BrightnessScreen"); 
            loading.SetActive(true);
        }
    }

    public void ChangeLocale(int localeID)
    {
        if (_active == true) { return; }

        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        _active = false;
        _defined = true;
    }
}
