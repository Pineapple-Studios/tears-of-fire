using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System;

public class PauseManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] Canvas pauseMenu;
    [SerializeField] string sceneName;
    //[SerializeField] public GameObject eventSystem;
    [Header("Actions")]
    [SerializeField] public InputActionAsset Actions;

    [Header("Animator")]
    [SerializeField] public Animator transition;


    private void Start()
    {
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
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
        Debug.Log("Despausou");
        pauseMenu.gameObject.SetActive(false);
        Cursor.visible = false;
        OnTransitionBack();
        Time.timeScale = 1;
    }

    private void Pause()
    {
        Debug.Log("Pausou");
        Cursor.visible = true;
        pauseMenu.gameObject.SetActive(true);
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
}

