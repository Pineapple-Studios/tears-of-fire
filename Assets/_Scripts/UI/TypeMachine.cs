using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class TypeMachine : MonoBehaviour
{
    [SerializeField] string txtPT;
    [SerializeField] string txtEN;
    [SerializeField] TMP_Text view;
    char[] character;


    // Start is called before the first frame update
    void Start()
    {
        VerifyLanguage();
        Cursor.visible = true;
    }
    public IEnumerator ShowingTextEN()
    {
        if (gameObject.activeInHierarchy)
        {
            int count = 0;
            while (count < character.Length)
            {
                yield return new WaitForSeconds(0.1f);
                view.text += character[count];
                count++;
                Debug.Log(txtEN);
            }
        }
    }

    public IEnumerator ShowingTextPT()
    {
        if (gameObject.activeInHierarchy)
        {
            int count = 0;
            while (count < character.Length)
            {
                yield return new WaitForSeconds(0.1f);
                view.text += character[count];
                count++;
                Debug.Log(txtPT);
            }
        }
    }

    void VerifyLanguage()
    {
        string loc = LocalizationSettings.SelectedLocale.name;

        if (loc == "English (en)" && txtEN != null)
        {
            character = txtEN.ToCharArray();
            ShowingTextEN();
        }
        else
        {
            character = txtPT.ToCharArray();
            ShowingTextEN();
        }
    }
}
