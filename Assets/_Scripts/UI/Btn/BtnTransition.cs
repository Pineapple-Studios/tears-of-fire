using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BtnTransition: MonoBehaviour
{

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject canvas;

    [SerializeField] public InputActionAsset Actions;


    private void Start()
    {
        canvas.SetActive(false);
    }

    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Confirm").started += OnTransition;

    }

    private void OnEnable()
    {
        Actions.FindActionMap("UI").Enable();
    }

    private void OnDisable()
    {
        Actions.FindActionMap("UI").Disable();
    }

    public void OnTransition(InputAction.CallbackContext context)
    {
        Debug.Log(canvas);
        HandleBtn();
        transition.Play("anim_Rotation");
    }

    public void HandleBtn()
    {
        canvas.SetActive(true);
    }
}
