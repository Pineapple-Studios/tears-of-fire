using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    [Header("Inputs")]
    [SerializeField] public Canvas pauseMenu;
    [SerializeField] string sceneName;
    //[SerializeField] public GameObject eventSystem;
    [Header("Actions")]
    [SerializeField] public InputActionAsset Actions;

    [Header("Animator")]
    [SerializeField] public Animator transition;

    [Header("Txt In Progress")]
    [SerializeField]public TMP_Text txtIP;

    private PlayerInputHandler _pih;
    private PauseSettings _ps;

    private void Start()
    {
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        txtIP.gameObject.SetActive(false);
    }

    void Awake()
    {
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        _pih = FindAnyObjectByType<PlayerInputHandler>();
        _ps = FindAnyObjectByType<PauseSettings>();
    }

    public void OnEnable()
    {
        if (_pih != null) { _pih.KeyPauseDown += OnPause; }
    }

    public void OnDisable()
    {
        if (_pih != null) { _pih.KeyPauseDown -= OnPause; }
    }

    private void OnPause()
    {
        if (SceneManager.GetActiveScene().name.Contains(sceneName))
        {
            if (pauseMenu.gameObject.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        OnTransitionBack();
        _pih.EnableInputs();
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    private void Pause()
    {
        Cursor.visible = true;
        pauseMenu.gameObject.SetActive(true);
        txtIP.gameObject.SetActive(false);
        OnTransition();
        Time.timeScale = 0;
        _ps.HandlerCanvasStart();
    }

    public void OnTransition()
    {
        transition.Play("anim_Pause");
    }

    public void OnTransitionBack()
    {
        transition.Play("anim_ReversePause");
    }

    public void InProgress()
    {
        txtIP.gameObject.SetActive(true);
    }
}

