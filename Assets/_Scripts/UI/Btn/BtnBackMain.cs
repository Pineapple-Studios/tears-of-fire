using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBackMain : MonoBehaviour
{
    [Tooltip("Nome da cena que esse botão irá navegar")]
    [SerializeField] string SceneName;

    [SerializeField] Button _goToScreenButton;
    //[SerializeField] public Animator transition;
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

    public void OnTransition()
    {
        Debug.Log("BackMain");
        //transition.SetBool("is_Rotation", true);
        //PlayerPrefs.SetString("@tof/SceneName", SceneName);
        //transition.Play("clip_home");

        Destroy(FeedbackAndDebugManager.Instance.gameObject);
        Destroy(LevelDataManager.Instance.gameObject);
        Destroy(FMODAudioManager.Instance.gameObject);
        Destroy(CinemachineShakeManager.Instance.gameObject);
        Destroy(ScenarioColorManager.Instance.gameObject);

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
    }
}
