using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBack : MonoBehaviour
{

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] GameObject overlaidTxt;


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
        Debug.Log("TransitionBack");
        transition.SetBool("is_ReverseRotation", true);
        StartCoroutine(HandleBtn());
        transition.Play("anim_ReverseRotation");
    }

    private IEnumerator HandleBtn()
    {
        yield return new WaitForSeconds(1.5f);
        overlaidTxt.SetActive(false);
    }
}
