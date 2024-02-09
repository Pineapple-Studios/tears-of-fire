using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelectorTest : MonoBehaviour
{
    [SerializeField] GameObject dialogueManagerEN;
    [SerializeField] GameObject dialogueManagerPT;

    private bool _active = false;

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
    }

    public void SetectPT()
    {
        dialogueManagerEN.SetActive(false);
        dialogueManagerPT.SetActive(true);
    }

    public void SetectEN()
    {
        dialogueManagerEN.SetActive(true);
        dialogueManagerPT.SetActive(false);
    }
}
