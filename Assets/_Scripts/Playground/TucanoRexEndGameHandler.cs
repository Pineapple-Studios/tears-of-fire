using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TucanoRexEndGameHandler : MonoBehaviour
{
    private const float WHITE_SHINNY_SCREEN = 200f;

    [Header("Final hit props")]
    [SerializeField]
    private float _finalHitDuration = 1f;
    [SerializeField]
    private float _finalHitAmplitude = 5f;
    [SerializeField]
    private float _finalHitFrequency = 5f;
    [SerializeField]
    private float _finalTimeScale = 0.1f;

    [Header("References")]
    [SerializeField]
    private TucanoRex _kwy;
    [SerializeField]
    private GameObject _cvFeedback;

    [Header("Elements handlers")]
    [SerializeField]
    private GameObject[] _elementsToDeactive;
    [SerializeField]
    private GameObject[] _elementsToActive;

    private PlayerController _pc;
    private PlayerInputHandler _playerInputHandler;
    private AchievmentHandler _ah;
    private TucanoRexProps _trp;

    private Camera _activeCamera;
    private AutoExposure _autoExposure;
    private float _originalShinny = 0;
    private bool _isBackingToNormalLight = false;
    private bool _hasFinishedBoss = false;

    private void Awake()
    {
        _pc = FindAnyObjectByType<PlayerController>();
        _playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
        _ah = FindAnyObjectByType<AchievmentHandler>();
        _trp = FindAnyObjectByType<TucanoRexProps>();
    }

    private void OnEnable()
    {
        TucanoRexProps.onTucanoRexDead += OnTucanoRexDead;
        if (_playerInputHandler != null)
        {
            _playerInputHandler.KeyNPCInteractionDown += RoolBackToGame;
        }
    }

    private void OnDisable()
    {
        TucanoRexProps.onTucanoRexDead -= OnTucanoRexDead;
        if (_playerInputHandler != null)
        {
            _playerInputHandler.KeyNPCInteractionDown -= RoolBackToGame;
        }
    }

    private void OnTucanoRexDead(GameObject obj)
    {
        StartCoroutine("BossFightEndCoroutine");
    }

    private IEnumerator BossFightEndCoroutine()
    {
        _playerInputHandler.DisableInputsOnDialog();
        Time.timeScale = _finalTimeScale;
        CinemachineShakeManager.Instance.ShakeCamera(_finalHitAmplitude, _finalHitDuration, _finalHitFrequency);
        yield return new WaitForSeconds(_finalHitDuration);
        _cvFeedback.SetActive(true);
        DeactiveElements();
        _ah.SetCompleteState(_ah.TUCANOREX);
        ShinnyScreen();
    }

    private void Update()
    {
        if (
            _isBackingToNormalLight == false &&
            _activeCamera != null &&
            _autoExposure != null &&
            _autoExposure.keyValue.value != WHITE_SHINNY_SCREEN
        )
        {
            if (_originalShinny == 0) _originalShinny = _autoExposure.keyValue.value;
            _autoExposure.keyValue.value = Mathf.Lerp(_autoExposure.keyValue.value, WHITE_SHINNY_SCREEN, Time.deltaTime);
        }

        if (_isBackingToNormalLight)
        {
            _autoExposure.keyValue.value = Mathf.Lerp(_autoExposure.keyValue.value, _originalShinny, Time.deltaTime);
        }
    }

    private void ShinnyScreen()
    {
        CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (cinemachineBrain != null) _activeCamera = cinemachineBrain.OutputCamera;

        PostProcessVolume[] postProcessVolumes = FindObjectsOfType<PostProcessVolume>();
        if (postProcessVolumes.Length == 0) return;

        
        foreach (PostProcessVolume p in postProcessVolumes)
        {
            if (_autoExposure != null) continue;
            if (p.profile.TryGetSettings(out _autoExposure))
                _autoExposure = p.profile.GetSetting<AutoExposure>();
        }
    }

    private void BackToOrigin()
    {
        if (!_autoExposure) return;
        if (_hasFinishedBoss) return;
        
        _pc.transform.position = new Vector3(144.4f, 132.8f, 0);
        Time.timeScale = 1f;
        _isBackingToNormalLight = true;
    }

    private void DeactiveElements()
    {
        foreach (GameObject element in _elementsToDeactive)
        {
            element.SetActive(false);
        }
    }

    private void RoolBackToGame()
    {
        if (!_kwy.IsStarted()) return;
        if (_trp.GetCurrentLife() > 0) return;
        if (_hasFinishedBoss) return;

        BackToOrigin();
        _cvFeedback.SetActive(false);
        ActiveElements();
        // Enable dash
        LevelDataManager.Instance.SetDashState(true);
        _playerInputHandler.EnableInputs();
        _hasFinishedBoss = true;
    }

    private void ActiveElements()
    {
        foreach(GameObject element in _elementsToActive)
        {
            element.SetActive(true);
        }
    }
}
