using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private GameObject _goCheckpoint;
    [SerializeField]
    private GameObject _checkpointButtonGroup;

    [Header("Feedback labels")]
    [SerializeField]
    private TMP_Text _dashStateLabel;
    [SerializeField]
    private TMP_Text _kwyRoomKeyStateLabel;
    [SerializeField]
    private TMP_Text _infinityLifeStateLabel;

    public struct SCheckpoint
    {
        public string _name;
        public Vector3 _pos;

        public SCheckpoint(string name, Vector3 pos)
        {
            _name = name;
            _pos = pos;
        }
    }

    private List<SCheckpoint> _checkpoints = new();

    private void Start()
    {
        SaveAllCheckpoints();
        HandlePanel();
        HandleSceneButtons();
        HandleCheckpointButtons();
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

    private void HandleCheckpointButtons()
    {
        if (_checkpoints.Count == 0) return;
        foreach (SCheckpoint check in _checkpoints)
        {
            GameObject btnCheckpoint = Instantiate(_goCheckpoint, _checkpointButtonGroup.transform);
            btnCheckpoint.GetComponent<GoToCheckpoint>().CheckpointName = check._name;
            btnCheckpoint.GetComponent<GoToCheckpoint>().Pos = check._pos;
        }
    }

    private void ShowFeedbackPlayerEngines()
    {
        // Controlando label do dash
        _dashStateLabel.text = _dataController.IsDashEnabled || LevelDataManager.Instance.GetDashStatus() ? "Dash is ENABLED" : "Dash is DISABLED";
        // Controlando label da vida infinita
        _infinityLifeStateLabel.text = _dataController.IsInifinityLife ? "InifityLife is ENABLED" : "InifityLife is DISABLED";
        // Controlando label da chave da sala do key
        _kwyRoomKeyStateLabel.text = _dataController.HasKwyRoomsKey ? "WITH kwy room's key" : "WITHOT Kwy room's key";
    }

    private void HandlePanel()
    {
        if (_dataController.IsDebugPanelActive) _debugPanel.SetActive(true);
        else _debugPanel.SetActive(false);
    }

    private void SaveAllCheckpoints()
    {
        Checkpoint[] _checks = FindObjectsOfType<Checkpoint>();
        foreach(Checkpoint check in _checks)
        {
            SCheckpoint tmpCheck = new SCheckpoint(
                check.gameObject.transform.parent.gameObject.name, 
                check.GetSpawnPoint().position
            );

            _checkpoints.Add(tmpCheck);
        }
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
    /// Retorna se a vida infinita est� ativada ou n�o
    /// </summary>
    public bool IsInfinityLifeActive()
    {
        return _dataController.IsInifinityLife;
    }

    public void ToggleKwyKey()
    {
        _dataController.HasKwyRoomsKey = !_dataController.HasKwyRoomsKey;
        LevelDataManager.Instance.SetKwyRoomKey(_dataController.HasKwyRoomsKey);
    }

}
