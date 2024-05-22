using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quit : MonoBehaviour
{
    [SerializeField] GameObject cvPopUP;
    [SerializeField] GameObject btnNo;
    [SerializeField] GameObject btnQuit;

    private void Start()
    {
        cvPopUP.SetActive(false);
    }
    public void PopUpConfirmation()
    {
        cvPopUP.SetActive(true);
        EventSystem.current.SetSelectedGameObject(btnNo);
    }

    public void PopUpDenial()
    {
        cvPopUP.SetActive(false);
        EventSystem.current.SetSelectedGameObject(btnQuit);
    }

    public void OnQuit()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }
}
