using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    //[SerializeField] GameObject cvMain;
    [Header("Game Objects")]
    [SerializeField] GameObject cvAudio;
    [SerializeField] GameObject cvVideo;
    [SerializeField] GameObject btVideo;
    [SerializeField] GameObject btAudio;
    [Header("Actions")]
    [SerializeField] public InputActionAsset Actions;

    // Start is called before the first frame update
    void Start()
    {
        UiHandler(cvVideo, false);
        UiHandler(cvAudio, false);
        UiHandler(btAudio, true);
        UiHandler(btVideo, true);
    }

    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Cancel").performed += OnBack;
    }

    public void OnEnable()
    {
        Actions.FindActionMap("UI").Enable();
    }

    public void OnDisable()
    {
        Actions.FindActionMap("UI").Disable();
    }

    public void btnVideo()
    {
        UiHandler(cvVideo, true);
        UiHandler(cvAudio, false);
        UiHandler(btAudio, false);
        UiHandler(btVideo, false);
    }

    public void btnAudio()
    {
        UiHandler(cvVideo, false);
        UiHandler(cvAudio, true);
        UiHandler(btAudio, false);
        UiHandler(btVideo, false);
    }

    void UiHandler(GameObject ui, bool isActive)
    {
        ui.gameObject.SetActive(isActive);
    }

    void OnBack(InputAction.CallbackContext context)
    {
        if (cvVideo == true)
        {
            UiHandler(btAudio, true);
            UiHandler(btVideo, true);
            UiHandler(cvVideo, false);
        }
        if (cvAudio == true)
        {
            UiHandler(btAudio, true);
            UiHandler(btVideo, true);
            UiHandler(cvAudio, false);
        }
    }
}
