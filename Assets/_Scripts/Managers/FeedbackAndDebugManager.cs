using TMPro;
using UnityEditor;
using UnityEngine;

public class FeedbackAndDebugManager : MonoBehaviour
{ 
    public static FeedbackAndDebugManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    [Header("Database")]
    [SerializeField]
    private FeedbackAndDebugScriptableObject _dataController;

    [Header("Elements os DebugTools")]
    [SerializeField]
    private GameObject _debugPanel;
    [SerializeField]
    private GameObject _sceneButtonGroup;
    [SerializeField]
    private GameObject _goToPrefab;

    [Header("Feedback labels")]
    [SerializeField]
    private TMP_Text _dashStateLabel;
    [SerializeField]
    private TMP_Text _infinityLifeStateLabel;

    private void Start()
    {
        HandlePanel();
        HandleSceneButtons();
    }

    private void Update()
    {
        if (_debugPanel == null || _dataController == null) return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _dataController.IsDebugPanelActive = !_dataController.IsDebugPanelActive;
            HandlePanel();
        }

        
        ShowFeedbackPlayerEngines();
    }

    private void HandleSceneButtons()
    {
        if (_dataController.SceneList.Count == 0) return;
        foreach(string scene in _dataController.SceneList)
        {
            GameObject goTo = Instantiate(_goToPrefab, _sceneButtonGroup.transform);
            goTo.GetComponent<GoTo>().SceneName = scene;
            goTo.GetComponentInChildren<TMP_Text>().text = scene;
        }
    }

    private void ShowFeedbackPlayerEngines()
    {
        // Controlando label do dash
        _dashStateLabel.text = _dataController.IsDashEnabled || LevelDataManager.Instance.GetDashStatus() ? "Dash is ENABLED" : "Dash is DISABLED";
        // Controlando label da vida infinita
        _infinityLifeStateLabel.text = _dataController.IsInifinityLife ? "InifityLife is ENABLED" : "InifityLife is DISABLED";
    }

    private void HandlePanel()
    {
        if (_dataController.IsDebugPanelActive) _debugPanel.SetActive(true);
        else _debugPanel.SetActive(false);
    }


    /// <summary>
    /// Controlador do estado do dash
    /// </summary>
    /// <param name="state">Estado do dash</param>
    public void ToggleDashState()
    {
        _dataController.IsDashEnabled = !_dataController.IsDashEnabled;
        LevelDataManager.Instance.SetDashState(_dataController.IsDashEnabled);
    }

    /// <summary>
    /// Controlador do estado de vida infinita
    /// </summary>
    public void ToggleInfinityLife()
    {
        _dataController.IsInifinityLife = !_dataController.IsInifinityLife;
    }

    /// <summary>
    /// Retorna se a vida infinita está ativada ou não
    /// </summary>
    public bool IsInfinityLifeActive()
    {
        return _dataController.IsInifinityLife;
    }
}
