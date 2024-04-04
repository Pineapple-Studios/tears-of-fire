using UnityEngine;
using Cinemachine;

public class CinemachineShakeManager : MonoBehaviour
{
    public static CinemachineShakeManager Instance { get; private set; }

    [SerializeField]
    private float _defaultHitAmplitude = 5f;
    [SerializeField]
    private float _defaultHitFrequency = 5f;
    [SerializeField]
    private float _defaultShakeTime = 1f;

    private CinemachineStateDrivenCamera _csdc;

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

        _csdc = FindAnyObjectByType<CinemachineStateDrivenCamera>();
        DontDestroyOnLoad(gameObject);
    }

    private CinemachineVirtualCamera _currentCamera;
    private float _shakeTimer = 0f;
    private CinemachineVirtualCamera _tmpCam;

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                SetAmplitude(0f);
            }
        }
    }

    private void SetAmplitude(float amplitude)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = amplitude;
    }

    private void SetFrequency(float frequency)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_FrequencyGain = frequency;
    }

    public void ShakeCamera(float amplitude, float time)
    {
        SetAmplitude(amplitude);
        _shakeTimer = time;
    }

    public void ShakeCamera(float amplitude, float time, float frequency)
    {
        SetFrequency(frequency);
        ShakeCamera(amplitude, time);
    }

    public void ShakeCamera()
    {
        ShakeCamera(_defaultHitAmplitude, _defaultShakeTime, _defaultHitFrequency);
    }

    public void SetCurrentCamera()
    {
        _tmpCam = _csdc.LiveChild.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (_currentCamera != _tmpCam) _currentCamera = _tmpCam;
    }
}