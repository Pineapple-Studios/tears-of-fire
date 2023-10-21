using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnRotation : MonoBehaviour
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
        Debug.Log("AAAAAAAAAAAAAAA");
        //transition.SetBool("is_Rotation", true);
        HandleBtn();
        //PlayerPrefs.SetString("@tof/SceneName", cv);
        transition.Play("anim_Rotation");
    }

    public void HandleBtn()
    {
        canvas.SetActive(true);
    }
}
