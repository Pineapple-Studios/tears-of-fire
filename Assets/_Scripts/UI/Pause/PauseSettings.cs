using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettings : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] GameObject cvMain; 
    [SerializeField] GameObject cvSettings;
    [SerializeField] GameObject cvAudio;
    [SerializeField] GameObject cvVideo;
    [SerializeField] GameObject cvPopUp;

    [Header("Buttons")]
    [SerializeField] Button btnMenu;
    [SerializeField] Button btnSettings;
    [SerializeField] Button btnAudio;
    [SerializeField] Button btnVideo;
    [SerializeField] Button btnNo;

    [SerializeField] TMP_Text txtIP;

    void Start()
    {
        HandlerCanvasStart();
        FMODAudioManager.Instance.SetInitialValues();
    }

    public void OnEnable()
    {
        btnMenu.onClick.AddListener(delegate { PopUpConfirmation(); });
        btnSettings.onClick.AddListener(delegate { HandlerCanvasSettings(); });
        btnAudio.onClick.AddListener(delegate { HandlerCanvasAudio(); });
        btnVideo.onClick.AddListener(delegate { HandlerCanvasVideo(); });
        btnNo.onClick.AddListener(delegate { HandlerCanvasStart(); });
    }

    public void OnDisable()
    {
        btnMenu.onClick.RemoveListener(delegate { PopUpConfirmation(); });
        btnSettings.onClick.RemoveListener(delegate { HandlerCanvasSettings(); });
        btnAudio.onClick.RemoveListener(delegate { HandlerCanvasAudio(); });
        btnVideo.onClick.RemoveListener(delegate { HandlerCanvasVideo(); });
        btnNo.onClick.RemoveListener(delegate { HandlerCanvasStart(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UiHandler(GameObject cv, bool isActive)
    {
        cv.gameObject.SetActive(isActive);
    }

    public void HandlerCanvasStart()
    {
        cvMain.SetActive(true);
        UiHandler(cvSettings, false);
        UiHandler(cvAudio, false);
        UiHandler(cvVideo, false);
        UiHandler(cvPopUp, false);
    }

    public void HandlerCanvasSettings()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, true);
        UiHandler(cvAudio, false);
        UiHandler(cvVideo, false);
        UiHandler(cvPopUp, false);
        txtIP.gameObject.SetActive(false);
    }

    public void HandlerCanvasAudio()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, false);
        UiHandler(cvAudio, true);
        UiHandler(cvVideo, false);
        UiHandler(cvPopUp, false);
    }

    public void HandlerCanvasVideo()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, false);
        UiHandler(cvAudio, false);
        UiHandler(cvVideo, true);
        UiHandler(cvPopUp, false);
    }

    public void PopUpConfirmation()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, false);
        UiHandler(cvAudio, false);
        UiHandler(cvVideo, false);
        UiHandler(cvPopUp, true);
        txtIP.gameObject.SetActive(false);
    }
}
