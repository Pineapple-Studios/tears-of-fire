using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneJumper : MonoBehaviour
{
    [SerializeField]
    VideoPlayer cutscene;
    [SerializeField]
    private Animator _transitionAnimator;

    [Header("Joystick Inputs")]
    [SerializeField]
    private InputActionAsset _actionAsset;

    private InputActionMap _uiMap;

    private InputAction _confirm;

    private void Awake()
    {
        _uiMap = _actionAsset.FindActionMap("UI");
        if (_uiMap == null) return;

        _confirm = _uiMap.FindAction("Confirm");
        _confirm.performed += CutsceneHasEnded;
    }

    private void OnEnable()
    {
        if (_uiMap != null) _uiMap.Enable();
    }

    private void OnDisable()
    {
        if (_uiMap != null) _uiMap.Disable();
    }

    void Start()
    {
        cutscene.loopPointReached += CutsceneHasEnded;
    }

    void CutsceneHasEnded(VideoPlayer vp) { JumpCutscene(); }
    void CutsceneHasEnded(InputAction.CallbackContext context) { JumpCutscene(); }

    private void JumpCutscene()
    {
        if (_transitionAnimator != null) _transitionAnimator.SetBool("is_FireTr", true);
    }

    public void OnEndAnimation()
    {
        if (SceneManager.GetActiveScene().name == "BeforeGame")
            SceneManager.LoadScene("Tutorial");
    }
}
