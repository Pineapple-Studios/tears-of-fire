using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class SetupInputActions : MonoBehaviour
{
    // Actions to be loaded based on Map serialized
    private const string KEYBOARD_MAP = "Keyboard";
    private const string JOYSTICK_MAP = "Joystick";

    [Header("Joystick Inputs")]
    [SerializeField] 
    private InputActionAsset _actionAsset;

    private InputActionMap _keyboardMap;
    private InputActionMap _joystickMap;

    private InputAction _keyboard;
    private InputAction _joystick;

    [Header("Images")]
    [SerializeField] 
    public Sprite ImageKeyboard;
    [SerializeField] 
    public Sprite ImageJoystick;

    Image image;

    void Awake()
    {
        _keyboardMap = _actionAsset.FindActionMap(KEYBOARD_MAP);
        _joystickMap = _actionAsset.FindActionMap(JOYSTICK_MAP);

        if (_keyboardMap == null || _joystickMap == null) return;

        _keyboard = _keyboardMap.FindAction("Keyboard");
        _keyboard.performed += OnKeyboard;

        _joystick = _joystickMap.FindAction("Joystick");
        _joystick.performed += OnJoystick;
    }

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.sprite = LocalStorage.GetIsUsingKeyboard(false) ? ImageKeyboard : ImageJoystick;
        this.enabled = true;
    }

    public void OnEnable()
    {
        if (_joystickMap != null) _joystickMap.Enable();
        if (_keyboardMap != null) _keyboardMap.Enable();
    }

    public void OnDisable()
    {
        if (_joystickMap != null) _joystickMap.Disable();
        if (_keyboardMap != null) _keyboardMap.Disable();
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

