using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BtnRotation : MonoBehaviour
{
    
    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject canvas;


    private void OnEnable()
    {
        _goToScreenButton.onClick.AddListener(OnTransition);

    }

    private void OnDisable()
    {
        _goToScreenButton.onClick.RemoveListener(OnTransition);
    }

    public void OnTransition()
    {
        Debug.Log("AAAAAAAAAAAAAAA");
        HandleBtn();
        transition.Play("anim_Rotation");
    }

    public void HandleBtn()
    {
        canvas.SetActive(true);
    }
}
