using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class SetupInputActions : MonoBehaviour
{

    //const string INPUT_ACTIONS = "@TOF_INPUT_ACTIONS";

    private const string KEYBOARD_NAME = "Keyboard";
    private const string JOYSTICK_NAME = "Joystick";

    [Header("Joystick Inputs")]
    [SerializeField] public InputActionAsset actions;

    InputActionMap actionMap;

    private InputAction _keyboard;
    private InputAction _joystick;

    [Header("Images")]
    [SerializeField] public Sprite ImageKeyboard;
    [SerializeField] public Sprite ImageJoystick;
    Image image;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.sprite = LocalStorage.GetIsUsingKeyboard(false) ? ImageKeyboard : ImageJoystick;
        this.enabled = true;
    }

    void Awake()
    {
        actionMap = actions.FindActionMap(KEYBOARD_NAME);
        actionMap = actions.FindActionMap(JOYSTICK_NAME);
        if (actionMap == null) return;

        _keyboard = actionMap.FindAction(KEYBOARD_NAME);
        _keyboard.performed += OnKeyboard;

        _joystick = actionMap.FindAction(JOYSTICK_NAME);
        _joystick.performed += OnJoystick;
    }

    public void OnEnable()
    {
        if (actionMap == null) return;
        actionMap.Enable();

    }

    public void OnDisable()
    {
        if (actionMap == null) return;
        actionMap.Disable();
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

