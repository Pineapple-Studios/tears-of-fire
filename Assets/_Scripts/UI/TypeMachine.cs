using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeMachine : MonoBehaviour
{
    [SerializeField] string txt;
    [SerializeField] TMP_Text view;
    char[] character;


    // Start is called before the first frame update
    void Start()
    {
        character = txt.ToCharArray();
        StartCoroutine(ShowingText());    
    }
    public IEnumerator ShowingText()
    {
        if (gameObject.activeInHierarchy)
        {
            int count = 0;
            while (count < character.Length)
            {
                yield return new WaitForSeconds(0.1f);
                view.text += character[count];
                count++;
                Debug.Log(txt);
            }
        }
    }
}
