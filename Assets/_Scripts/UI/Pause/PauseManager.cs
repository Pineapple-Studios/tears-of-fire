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


    private void Start()
    {
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        txtIP.gameObject.SetActive(false);

        _pih = FindAnyObjectByType<PlayerInputHandler>();
    }
    void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Pause").performed += OnPause;
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
    }

    public void OnEnable()
    {
        Actions.FindActionMap("UI").Enable();
    }

    public void OnDisable()
    {
        Actions.FindActionMap("UI").Disable();
    }

    void OnPause(InputAction.CallbackContext context)
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
        Debug.Log("Despausou");
        Cursor.visible = false;
        Time.timeScale = 1;
        //pauseMenu.gameObject.SetActive(false);
    }

    private void Pause()
    {
        Debug.Log("Pausou");
        _pih.DisableInputs();
        Cursor.visible = true;
        pauseMenu.gameObject.SetActive(true);
        txtIP.gameObject.SetActive(false);
        OnTransition();
        Time.timeScale = 0;
    }

    public void OnTransition()
    {
        Debug.Log("AnimPause");
        transition.Play("anim_Pause");
    }

    public void OnTransitionBack()
    {
        Debug.Log("AnimReversePause");
        transition.Play("anim_ReversePause");
    }

    public void InProgress()
    {
        txtIP.gameObject.SetActive(true);
    }
}

