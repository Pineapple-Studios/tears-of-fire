using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] public Canvas pauseMenu;
    [SerializeField] string sceneName;
    //[SerializeField] public GameObject eventSystem;
    [Header("Actions")]
    [SerializeField] public InputActionAsset Actions;

    [Header("Animator")]
    [SerializeField] public Animator transition;

    [Header("Txt In Progress")]
    [SerializeField] TMP_Text txtIP;


    private void Start()
    {
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        txtIP.gameObject.SetActive(false);
    }
    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Pause").performed += OnPause;
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
    }

    public void OnEnable()
    {
        Actions.FindActionMap("UI").Enable();
    }

    public void OnDisable()
    {
        Actions.FindActionMap("UI").Disable();
    }

    void OnPause(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name.Contains(sceneName))
        {
            if (pauseMenu.gameObject.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        OnTransitionBack();
        Debug.Log("Despausou");
        Cursor.visible = false;
        Time.timeScale = 1;
        //pauseMenu.gameObject.SetActive(false);
    }

    private void Pause()
    {
        Debug.Log("Pausou");
        Cursor.visible = true;
        pauseMenu.gameObject.SetActive(true);
        txtIP.gameObject.SetActive(false);
        OnTransition();
        Time.timeScale = 0;
    }

    public void OnTransition()
    {
        Debug.Log("AnimPause");
        transition.Play("anim_Pause");
    }

    public void OnTransitionBack()
    {
        Debug.Log("AnimReversePause");
        transition.Play("anim_ReversePause");
    }

    public void InProgress()
    {
        txtIP.gameObject.SetActive(true);
    }
}

