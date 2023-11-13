using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnSettings : MonoBehaviour
{

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject canvas;

    [SerializeField] public InputActionAsset Actions;

    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Confirm").performed += OnTransitionCallback;
    }

    private void OnEnable()
    {
        //_goToScreenButton?.onClick.AddListener(GoTo);
        _goToScreenButton.onClick.AddListener(OnTransition);
        Actions.FindActionMap("UI").Enable();
    }

    private void OnDisable()
    {
        //_goToScreenButton?.onClick.RemoveListener(GoTo);
        _goToScreenButton.onClick.RemoveListener(OnTransition);
        Actions.FindActionMap("UI").Disable();
    }

    private void GoTo()
    {
        Time.timeScale = 1;
    }

    public void OnTransitionCallback(InputAction.CallbackContext context)
    {
        OnTransition();
    }

    public void OnTransition()
    {
        transition.SetBool("is_Settings", true);
        HandleBtn();
        transition.Play("anim_Settings");
        Debug.Log("TransitionSettings");
    }

    public void HandleBtn()
    {
        canvas.SetActive(true);
    }
}
