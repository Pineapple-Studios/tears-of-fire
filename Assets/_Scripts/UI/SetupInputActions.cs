using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetupInputActions : MonoBehaviour
{
    // Actions to be loaded based on Map serialized
    private const string KEYBOARD_MAP = "Keyboard";
    private const string JOYSTICK_MAP = "Joystick";

    [Header("Joystick Inputs")]
    [SerializeField] 
    private InputActionAsset _actionAsset;

    private InputAction _keyboard;
    private InputAction _joystick;

    private InputActionMap _keyboardMap;
    private InputActionMap _joystickMap;


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
        SetSprite(LocalStorage.GetIsUsingKeyboard(false));
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
        LocalStorage.SetIsUsingKeyboard(true);
        SetSprite(true);
    }

    private void OnJoystick(InputAction.CallbackContext context)
    {
        LocalStorage.SetIsUsingKeyboard(false);
        SetSprite(false);
    }

    private void SetSprite(bool state)
    {
        image = GetComponent<Image>();
        image.sprite = state ? ImageKeyboard : ImageJoystick;
        image.SetNativeSize();
    }
}

