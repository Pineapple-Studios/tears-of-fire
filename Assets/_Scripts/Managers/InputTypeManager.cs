using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeManager : MonoBehaviour
{
    public static InputTypeManager Instance { get; private set; }

    // Actions to be loaded based on Map serialized
    private const string KEYBOARD_MAP = "Keyboard";
    private const string JOYSTICK_MAP = "Joystick";

    [Header("Joystick Inputs")]
    [SerializeField]
    private InputActionAsset _actionAsset;

    private InputActionMap _keyboardMap;
    private InputAction _keyboard;
    private InputActionMap _joystickMap;
    private InputAction _joystick;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }

        DontDestroyOnLoad(gameObject);

        _keyboardMap = _actionAsset.FindActionMap(KEYBOARD_MAP);
        _joystickMap = _actionAsset.FindActionMap(JOYSTICK_MAP);

        if (_keyboardMap == null || _joystickMap == null) return;

        _keyboard = _keyboardMap.FindAction("Keyboard");
        _keyboard.performed += OnKeyboard;

        _joystick = _joystickMap.FindAction("Joystick");
        _joystick.performed += OnJoystick;
    }

    public void OnEnable()
    {
        if (_joystickMap != null) _joystickMap.Enable();
        if (_keyboardMap != null) _keyboardMap.Enable();
    }

    public void OnDisable()
    {
        if (_joystickMap != null) _joystickMap.Disable();
        if(_keyboardMap != null) _keyboardMap.Disable();
    }

    private void OnKeyboard(InputAction.CallbackContext context)
    {
        LocalStorage.SetIsUsingKeyboard(true);
    }

    private void OnJoystick(InputAction.CallbackContext context)
    {
        LocalStorage.SetIsUsingKeyboard(false);
    }
}
