using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JumpScene : MonoBehaviour
{
    [Header("Joystick Inputs")]
    [SerializeField] public InputActionAsset _actionAsset;

    [SerializeField] Animator anim;

    private InputActionMap _uiMap;

    private InputAction _confirm;

    private void Awake()
    {
        _uiMap = _actionAsset.FindActionMap("UI");
        if (_uiMap == null) return;

        _confirm = _uiMap.FindAction("Confirm");
        _confirm.performed += OnConfirm;
    }

    private void OnEnable()
    {
        if (_uiMap != null) _uiMap.Enable();
    }

    private void OnDisable()
    {
        if (_uiMap != null) _uiMap.Disable();
    }

    void OnConfirm(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().name == "BrightnessScreen")
        {
            FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.clickUI, this.transform.position);
            anim.Play("clip_Loading");
        }
    }

    void OnCancel(InputAction.CallbackContext context)
    {

    }
}
