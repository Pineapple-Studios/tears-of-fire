using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnGoToScreen : MonoBehaviour
{
    [Tooltip("Nome da cena que esse botão irá navegar")]
    [SerializeField] public string SceneName;
    [SerializeField] public string clipName;

    [SerializeField] Button _goToScreenButton;
    [SerializeField] public Animator transition;
    [SerializeField] public GameObject overlaidTxt;


    void Start()
    {
        //PlayerPrefs.SetString("@tof/SceneName", SceneName);
        if(SceneManager.GetActiveScene().name == "BeforeGame" || SceneManager.GetActiveScene().name == "AfterGame") { Cursor.visible = true; }
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

    public void OnTransition()
    {
        Debug.Log("TranitionFire");
        //transition.SetBool("is_Rotation", true);
        //PlayerPrefs.SetString("@tof/SceneName", SceneName);
        transition.Play(clipName);
        //SceneManager.LoadScene(SceneName);
    }
}
