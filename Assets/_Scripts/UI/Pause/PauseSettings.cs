using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettings : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] GameObject cvMain; 
    [SerializeField] GameObject cvSettings;
    [SerializeField] GameObject cvAudio;
    [SerializeField] GameObject cvVideo;

    [Header("Buttons")]
    [SerializeField] Button btnSettings;
    [SerializeField] Button btnAudio;
    [SerializeField] Button btnVideo;

    void Start()
    {
        HandlerCanvasStart();
    }

    public void OnEnable()
    {
        btnSettings.onClick.AddListener(delegate { HandlerCanvasSettings(); });
        btnAudio.onClick.AddListener(delegate { HandlerCanvasAudio(); });
        btnVideo.onClick.AddListener(delegate { HandlerCanvasVideo(); });
    }

    public void OnDisable()
    {
        btnSettings.onClick.RemoveListener(delegate { HandlerCanvasSettings(); });
        btnAudio.onClick.RemoveListener(delegate { HandlerCanvasAudio(); });
        btnVideo.onClick.RemoveListener(delegate { HandlerCanvasVideo(); });
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
    }

    public void HandlerCanvasSettings()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, true);
        UiHandler(cvAudio, false);
        UiHandler(cvVideo, false);
    }

    public void HandlerCanvasAudio()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, false);
        UiHandler(cvAudio, true);
        UiHandler(cvVideo, false);
    }

    public void HandlerCanvasVideo()
    {
        cvMain.SetActive(false);
        UiHandler(cvSettings, false);
        UiHandler(cvAudio, false);
        UiHandler(cvVideo, true);
    }
}
