using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class SetupInputActions : MonoBehaviour
{

    const string INPUT_ACTIONS = "@BERRYRUSH_INPUT_ACTIONS";

    [Header("Joystick Inputs")]
    [SerializeField] public InputActionAsset actions;
    [SerializeField] public Sprite ImageKeyboard;
    [SerializeField] public Sprite ImageJoystick;
    Image image;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.sprite = LocalStorage.GetIsUsingKeyboard(false) ? ImageKeyboard : ImageJoystick;
    }

    void Awake()
    {
        actions.FindActionMap("Keyboard").FindAction("Keyboard").performed += OnKeyboard;
        //actions.FindActionMap("Joystick").FindAction("Joystick").performed += OnJoystick;
    }

    public void OnEnable()
    {
        actions.FindActionMap("Keyboard").Enable();
        //actions.FindActionMap("Joystick").Enable();

    }

    public void OnDisable()
    {
        actions.FindActionMap("Keyboard").Disable();
        //actions.FindActionMap("Joystick").Disable();
    }

    private void OnKeyboard(InputAction.CallbackContext context)
    {
        image.sprite = ImageKeyboard;
        LocalStorage.SetIsUsingKeyboard(true);
    }

    private void OnJoystick(InputAction.CallbackContext context)
    {
        image.sprite = ImageJoystick;
        LocalStorage.SetIsUsingKeyboard(false);
    }

}

