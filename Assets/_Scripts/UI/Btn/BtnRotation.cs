using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnRotation : MonoBehaviour
{
    
    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject canvas;

    [SerializeField] public InputActionAsset Actions;

    void Awake()
    {
        //Actions.FindActionMap("UI").FindAction("Confirm").performed += OnTransitionCallback;
    }

    private void OnEnable()
    {
        _goToScreenButton.onClick.AddListener(OnTransition);
        //Actions.FindActionMap("UI").Enable();
    }

    private void OnDisable()
    {
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
        Debug.Log("AAAAAAAAAAAAAAA");
        HandleBtn();
        transition.Play("anim_Rotation");
    }

    public void HandleBtn()
    {
        canvas.SetActive(true);
    }
}
