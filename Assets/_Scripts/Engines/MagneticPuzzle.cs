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

    private void OnEnable()
    {
        LevelDataManager.onRestartElements += ResetPuzzle;
    }

    private void OnDisable()
    {
        LevelDataManager.onRestartElements -= ResetPuzzle;
    }

    private void Start()
    {
        SetPlataformToFirstHookPosition();
    }

    private void Update()
    {
        if (_inMoviment) GoAheadByVelocity();
        if (_isGoingBack) GoBackByVelocity();

        CounterToBackStep();
    }

    private void ResetPuzzle()
    {
        SetPlataformToFirstHookPosition();

        _currentHook = 0;
        _stepBackTimer = 0f;
        _inMoviment = false;
        _isGoingBack = false;
        _platform.GetComponent<AffectPlayerMovement>().DisableAffect();
    }

    /// <summary>
    /// Posiciona a plataforma na primeira âncora
    /// </summary>
    private void SetPlataformToFirstHookPosition()
    {
        if (_anchorPoints.Count == 0 || _platform == null) return;

        _platform.transform.position = _anchorPoints[0].transform.position + _offsetPlatformPosition;
        _platform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    /// <summary>
    /// Gerenciador de estados da mecânica
    /// </summary>
    private void CounterToBackStep()
    {
        // Não conta tempo no estado inicial
        if (!_inMoviment && !_isGoingBack && _currentHook == 0) return;
        // Começa a contar o tempo após chegar na ancora 1
        if (!_inMoviment && !_isGoingBack && _currentHook > 0) _stepBackTimer += Time.deltaTime;

        if (!_inMoviment && _stepBackTimer > _timeToGoBack) _isGoingBack = true;
    }

    /// <summary>
    /// Retorna a plataforma para a ancora anterior
    /// </summary>
    private void GoBackByVelocity()
    {
        Vector3 targetPos = _anchorPoints[_currentHook - 1].transform.position + _offsetPlatformPosition;
        if (_dir == Vector3.zero) _dir = targetPos - _platform.transform.position;

        _platform.GetComponent<Rigidbody2D>().velocity = _dir * _velocityPoints * Time.deltaTime;

        if (_currentHook - 1 >= 0 && _platform.transform.position.y <= targetPos.y)
        {
            _platform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _dir = Vector3.zero;
            _stepBackTimer = 0f;
            _isGoingBack = false;
            _currentHook--;
        }
    }

    /// <summary>
    /// Leva a plataforma para a próxima ancora
    /// </summary>
    public void GoToNextPoint()
    {
        // Bloqueandoa ações caso a plataforma estaja em movimento contrário
        if (_isGoingBack) return;
        _stepBackTimer = 0f;

        if (_anchorPoints.Count != _currentHook + 1) _inMoviment = true;
    }

    Vector3 _dir = Vector3.zero;
    /// <summary>
    /// Carrega a plataforma para a próxima ancora
    /// </summary>
    private void GoAheadByVelocity()
    {
        Vector3 targetPos = _anchorPoints[_currentHook + 1].transform.position + _offsetPlatformPosition;
        if (_dir == Vector3.zero) _dir = targetPos - _platform.transform.position;

        _platform.GetComponent<Rigidbody2D>().velocity = _dir * _velocityPoints * Time.deltaTime;

        if (_currentHook + 1 < _anchorPoints.Count && _platform.transform.position.y > targetPos.y)
        {
            _platform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _dir = Vector3.zero;
            _stepBackTimer = 0f;
            _inMoviment = false;
            _currentHook++;
        }
    }
}
