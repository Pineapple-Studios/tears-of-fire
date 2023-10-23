using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MagneticPuzzle : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField]
    private GameObject _platform;
    [SerializeField]
    private List<GameObject> _anchorPoints = new List<GameObject> {};

    [Header("Props")]
    [SerializeField]
    private float _velocityPoints = 2f;
    [SerializeField]
    private float _timeToGoBack = 2f;
    [SerializeField]
    private Vector3 _offsetPlatformPosition = Vector3.zero;

    private int _currentHook = 0;
    private float _stepBackTimer = 0f;
    private bool _inMoviment = false;
    private bool _isGoingBack = false;

    private void Start()
    {
        SetPlataformToFirstHookPosition();
    }

    private void Update()
    {
        if (_inMoviment) GoAhead();
        if (_isGoingBack) GoBack();

        CounterToBackStep();
    }

    /// <summary>
    /// Posiciona a plataforma na primeira �ncora
    /// </summary>
    private void SetPlataformToFirstHookPosition()
    {
        if (_anchorPoints.Count == 0 || _platform == null) return;

        _platform.transform.position = _anchorPoints[0].transform.position + _offsetPlatformPosition;
    }

    /// <summary>
    /// Carrega a plataforma para a pr�xima ancora
    /// </summary>
    private void GoAhead()
    {
        Vector3 targetPos = _anchorPoints[_currentHook + 1].transform.position + _offsetPlatformPosition;
        _platform.transform.position = Vector3.MoveTowards(
            _platform.transform.position, 
            targetPos, 
            _velocityPoints * Time.deltaTime
        );

        if (_currentHook + 1 < _anchorPoints.Count && _platform.transform.position == targetPos)
        {
            _stepBackTimer = 0f;
            _inMoviment = false;
            _currentHook++;
        }
    }

    /// <summary>
    /// Gerenciador de estados da mec�nica
    /// </summary>
    private void CounterToBackStep()
    {
        // N�o conta tempo no estado inicial
        if (!_inMoviment && !_isGoingBack && _currentHook == 0) return;
        // Come�a a contar o tempo ap�s chegar na ancora 1
        if (!_inMoviment && !_isGoingBack && _currentHook > 0) _stepBackTimer += Time.deltaTime;

        if (!_inMoviment && _stepBackTimer > _timeToGoBack) _isGoingBack = true;
    }

    /// <summary>
    /// Retorna a plataforma para a ancora anterior
    /// </summary>
    private void GoBack()
    {
        Vector3 targetPos = _anchorPoints[_currentHook - 1].transform.position + _offsetPlatformPosition;
        _platform.transform.position = Vector3.MoveTowards(
            _platform.transform.position,
            targetPos,
            _velocityPoints * Time.deltaTime
        );

        if (_platform.transform.position == targetPos)
        {
            _stepBackTimer = 0f;
            _isGoingBack = false;
            _currentHook--;
        }
    }

    /// <summary>
    /// Leva a plataforma para a pr�xima ancora
    /// </summary>
    public void GoToNextPoint()
    {
        // Bloqueandoa a��es caso a plataforma estaja em movimento contr�rio
        if (_isGoingBack) return;
        _stepBackTimer = 0f;

        if (_anchorPoints.Count != _currentHook + 1) _inMoviment = true;
    }
}