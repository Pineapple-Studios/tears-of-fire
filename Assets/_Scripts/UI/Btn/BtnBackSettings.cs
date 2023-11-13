using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBackSettings : MonoBehaviour
{

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject canvas;

    [SerializeField] public InputActionAsset Actions;

    void Awake()
    {
        //Actions.FindActionMap("UI").FindAction("Cancel").performed += OnTransitionCallback;
    }

    private void OnEnable()
    {
        //_goToScreenButton?.onClick.AddListener(GoTo);
        //Actions.FindActionMap("UI").Enable();
        _goToScreenButton.onClick.AddListener(OnTransition);
    }

    private void OnDisable()
    {
        //_goToScreenButton?.onClick.RemoveListener(GoTo);
        _goToScreenButton.onClick.RemoveListener(OnTransition);
        //Actions.FindActionMap("UI").Disable();
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
        Debug.Log("TransitionBack");
        transition.SetBool("is_ReverseSettings", true);
        transition.Play("anim_ReverseSettings");
    }

    public void HandleBtn()
    {
        canvas.SetActive(false);
    }
}
