using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VFXPlayerLightByLife : MonoBehaviour
{
    [SerializeField]
    private float _minLightRange = 0.2f;
    [SerializeField]
    private float _maxLightRange = 0.4f;
    [SerializeField]
    private float _transitionSpeed = 2f;

    // Shadow processing
    private Camera activeCamera;
    private Vector2 _positionRelatedToCam;
    private PostProcessVolume[] _postProcessVolumes;
    private Vignette _vignette;

    // Player Properties
    private PlayerProps _pp;
    private bool _isIgoreSetup = false;

    private void OnEnable()
    {
        StartBoss.OnStartedBoss += SetDefaultPresets;
        LevelDataManager.onRestartElements += Restart;
        TucanoRexProps.onTucanoRexDead += Restart;
        PlayerProps.onPlayerDead += Restart;
    }

    private void OnDisable()
    {
        StartBoss.OnStartedBoss -= SetDefaultPresets;
        LevelDataManager.onRestartElements -= Restart;
        TucanoRexProps.onTucanoRexDead -= Restart;
        PlayerProps.onPlayerDead -= Restart;
    }

    private void Restart()
    {
        _isIgoreSetup = false;
    }

    private void Restart(GameObject go) { Restart(); }

    private void SetDefaultPresets()
    {
        _isIgoreSetup = true;
        if (_vignette == null) return;

        _vignette.center.value = new Vector2(0.5f, 0.5f);
        _vignette.intensity.value = Mathf.Lerp(
            _vignette.intensity.value, 0.1f, Time.deltaTime * _transitionSpeed
        );
        _vignette.rounded.value = false;
    }

    void Start()
    {
        CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (cinemachineBrain != null) activeCamera = cinemachineBrain.OutputCamera;

        _postProcessVolumes = FindObjectsOfType<PostProcessVolume>();
        if (_postProcessVolumes.Length == 0)
        {
            Debug.LogError("PostProcessVolume not found in the scene.");
            return;
        }

        foreach (PostProcessVolume p in _postProcessVolumes) {
            if (_vignette != null) continue;
            if (p.profile.TryGetSettings(out _vignette)) 
                _vignette = p.profile.GetSetting<Vignette>();
        }

        _pp = GetComponent<PlayerProps>();
    }

    void Update()
    {
        if (_isIgoreSetup) return;

        if (activeCamera != null)
        {
            Vector3 screenPosition = activeCamera.WorldToScreenPoint(transform.position);
            _positionRelatedToCam = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
            if (_vignette != null) _vignette.center.value = _positionRelatedToCam;
            UpdateLigth();
        }
    }

    private float UnitOfLight() => (_maxLightRange - _minLightRange) / _pp.GetCurrentMaxLifeByUI();

    private void UpdateLigth()
    {
        if (_vignette == null || _pp == null) return;

        if (!_vignette.rounded.value) _vignette.rounded.value = true;
        _vignette.intensity.value = Mathf.Lerp(
            _vignette.intensity.value,
            _maxLightRange - (UnitOfLight() * _pp.GetLifeByUI()),
            Time.deltaTime * _transitionSpeed
        );
    }
}
