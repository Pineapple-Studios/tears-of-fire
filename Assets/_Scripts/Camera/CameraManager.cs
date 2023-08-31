using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField]
    private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Constrols for lerping the Y damping during playler jump/fall")]
    [SerializeField]
    private float _fallPanAmmount = 0.25f;
    [SerializeField]
    private float _fallYPanTime = 0.35f;

    public float _fallSpeedYDampingChangeThreshold = -15f;
    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framningTranspose;

    private float _normYPanAmmount;

    private void Awake()
    {
        if (instance == null) instance = this;

        for(int i = 0; i < _allVirtualCameras.Length; i++)
        {
            _currentCamera = _allVirtualCameras[i];

            _framningTranspose = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmmount = _framningTranspose.m_YDamping;
        float endDampAmmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmmount = _fallPanAmmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmmount = _normYPanAmmount;
        }


        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmmount = Mathf.Lerp(startDampAmmount, endDampAmmount, (elapsedTime / _fallYPanTime));
            _framningTranspose.m_YDamping = lerpedPanAmmount;

            yield return null;
        }


        IsLerpingYDamping = false;
    }
}
