using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBack : MonoBehaviour
{

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject overlaidTxt;

    [SerializeField] public InputActionAsset Actions;

    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Cancel").performed += OnTransitionCallback;
    }


    private void OnEnable()
    {
        //_goToScreenButton?.onClick.AddListener(GoTo);
        _goToScreenButton.onClick.AddListener(OnTransition);
        Actions.FindActionMap("UI").Enable();
    }

    private void OnDisable()
    {
        //_goToScreenButton?.onClick.RemoveListener(GoTo);
        _goToScreenButton.onClick.RemoveListener(OnTransition);
        Actions.FindActionMap("UI").Disable();
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
        Debug.Log("TransitionBack");
        transition.Play("anim_ReverseRotation");
    }


    private IEnumerator HandleBtn()
    {
        yield return new WaitForSeconds(1.5f);
        overlaidTxt.SetActive(false);
    }
}
