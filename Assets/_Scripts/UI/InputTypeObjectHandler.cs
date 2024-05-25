using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeObjectHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _keyboardElement;
    [SerializeField]
    private GameObject _joystickElement;

    private bool IsKeyboard = true;

    private void Start()
    {
        ActiveFeedbackElement(IsKeyboard);
    }

    private void Update()
    {
        if (IsKeyboard != LocalStorage.GetIsUsingKeyboard(IsKeyboard))
        {
            IsKeyboard = LocalStorage.GetIsUsingKeyboard(IsKeyboard);
            ActiveFeedbackElement(IsKeyboard);
        }
    }

    private void ActiveFeedbackElement(bool isKeyboard) 
    { 
        if (isKeyboard)
        {
            _joystickElement.SetActive(false);
            _keyboardElement.SetActive(true);
        }
        else
        {
            _keyboardElement.SetActive(false);
            _joystickElement.SetActive(true);
        }
    }
}
