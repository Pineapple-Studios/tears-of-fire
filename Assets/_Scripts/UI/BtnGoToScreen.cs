using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnGoToScreen : MonoBehaviour
{
    [Tooltip("Nome da cena que esse bot�o ir� navegar")]
    [SerializeField] string SceneName;

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;

    void Awake()
    {
        PlayerPrefs.SetString("@tof/SceneName", SceneName);
    }

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
        SceneManager.LoadScene(SceneName);
    }

    public void OnTransition()
    {
        Debug.Log("AAAAAAAAAAAAAAA");
        //transition.SetBool("is_FireTr", true);
        transition.Play("tion_FireTransition");
    }
}