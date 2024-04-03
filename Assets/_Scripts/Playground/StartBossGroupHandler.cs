using UnityEngine;

public class StartBossGroupHandler : MonoBehaviour
{
    [SerializeField]
    private StartBoss _first;
    [SerializeField]
    private StartBoss _second;

    private GameObject _firstGo;
    private GameObject _secondGo;
    private bool _hasFirstDone = false;

    private void OnEnable()
    {
        LevelDataManager.onRestartElements += Restart;
    }

    private void OnDisable()
    {
        LevelDataManager.onRestartElements -= Restart;
    }

    private void Restart()
    {
        if (_hasFirstDone) FirstInteractionActionDone();
        else Initialize();
    }

    void Start()
    {
        _firstGo = _first.gameObject;
        _secondGo = _second.gameObject;

        Initialize();
    }

    private void Initialize()
    {
        _firstGo.SetActive(true);
        _secondGo.SetActive(false);
    }

    public void FirstInteractionActionDone()
    {
        _hasFirstDone = true;
        _firstGo.SetActive(false);
        _secondGo.SetActive(true);
    }
}
