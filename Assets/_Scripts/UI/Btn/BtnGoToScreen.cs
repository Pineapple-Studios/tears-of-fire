using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnGoToScreen : MonoBehaviour
{
    [Tooltip("Nome da cena que esse botão irá navegar")]
    [SerializeField] string SceneName;

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] public GameObject overlaidTxt;


    void Awake()
    {
        //PlayerPrefs.SetString("@tof/SceneName", SceneName);
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
        Debug.Log("TranitionFire");
        //transition.SetBool("is_Rotation", true);
        PlayerPrefs.SetString("@tof/SceneName", SceneName);
        transition.Play("tion_FireTransition");
    }
}
