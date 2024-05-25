using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetupInputActions : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] 
    public Sprite ImageKeyboard;
    [SerializeField] 
    public Sprite ImageJoystick;

    private Image image;

    private bool _isKeyboard = true;

    void Start()
    {
        SetSprite(LocalStorage.GetIsUsingKeyboard(_isKeyboard));
    }

    private void Update()
    {
        if (_isKeyboard != LocalStorage.GetIsUsingKeyboard(_isKeyboard))
        {
            _isKeyboard = LocalStorage.GetIsUsingKeyboard(_isKeyboard);
            SetSprite(_isKeyboard);
        }
    }

    private void SetSprite(bool state)
    {
        image = GetComponent<Image>();
        image.sprite = state ? ImageKeyboard : ImageJoystick;
        image.SetNativeSize();
    }
}

