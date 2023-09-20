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
    [SerializeField] Canvas pauseMenu;
    //[SerializeField] public GameObject eventSystem;
    [Header("Actions")]
    [SerializeField] public InputActionAsset Actions;

    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Pause").performed += OnPause;
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
        if (SceneManager.GetActiveScene().name.Contains("PauseMenu"))
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
        //eventSystem.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void Pause()
    {
        //eventSystem.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}

