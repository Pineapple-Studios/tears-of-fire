using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnSettings : MonoBehaviour
{

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject canvas;


    private void OnEnable()
    {
        //_goToScreenButton?.onClick.AddListener(GoTo);
        _goToScreenButton.onClick.AddListener(OnTransition);
    }

    private void OnDisable()
    {
        //_goToScreenButton?.onClick.RemoveListener(GoTo);
        _goToScreenButton.onClick.RemoveListener(OnTransition);
    }

    private void GoTo()
    {
        Time.timeScale = 1;
    }

    public void OnTransition()
    {
        transition.SetBool("is_Settings", true);
        HandleBtn();
        transition.Play("anim_Settings");
        Debug.Log("TransitionSettings");
    }

    public void HandleBtn()
    {
        canvas.SetActive(true);
    }
}
