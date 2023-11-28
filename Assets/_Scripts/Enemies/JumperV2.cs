using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperV2 : MonoBehaviour
{
    [Header("Moviment")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Vector2 _forcesXY = new Vector2(10, 10);
    [SerializeField]
    private AnimationCurve _yDynamic;
    [SerializeField]
    private float _speed = 300f;
    [Tooltip("intervalo de tempo para o movimento")]
    [SerializeField]
    private float _cooldown = 1f;
    [SerializeField]
    private float _distance = 1f;
    
    [Header("Casting")]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _wallDistance;
    [SerializeField]
    private Vector3 _offsetToCheck;
    [SerializeField]
    private bool _isFacingRight = true;

    private bool _shouldMove = false;
    private bool _isGrounded = false;
    private bool _hasWallAhead = false;
    private bool _hasWallAheadMaxHeight = false;
    private float _timeCounter = 0;
    private Vector3 _initialPosition;
    private Vector3 _foward2D;
    private float _maxHeight = 0f;

    // Global
    private bool _enableEvents = false;

    private void Start()
    {
        Restart();
    }

    private void Update()
    {
        if (!_enableEvents) return;

        if (_foward2D == null) _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;

        // Iniciando pulo quando no chão
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _groundLayer);

        // verifica se tem uma parede a frente
        _hasWallAhead = Physics2D.Raycast(transform.position + _offsetToCheck, _foward2D, _distance + _wallDistance, _groundLayer);
        _hasWallAheadMaxHeight = _maxHeight != 0 && Physics2D.Raycast(transform.position + _offsetToCheck + new Vector3(0, _maxHeight, 0), _foward2D, _distance + _wallDistance, _groundLayer);

        // Debug.Log(transform.position + _offsetToCheck + new Vector3(0, _maxHeight, 0));

        if ((_hasWallAhead || _hasWallAheadMaxHeight) && !_shouldMove) FlipAndReset();

        if (!_shouldMove && _isGrounded) _timeCounter += Time.deltaTime;
        else _timeCounter = 0;

        
        if (_isGrounded && _shouldMove && Mathf.Abs(transform.position.x - _initialPosition.x) >= _distance)
        {
            _shouldMove = false;
            _initialPosition = transform.position;
        }
        

        if (_timeCounter >= _cooldown) _shouldMove = true;

        if (_shouldMove) Move();
        else Stop();
    }

    private void Flip()
    {
        Quaternion rot = Quaternion.Euler(0, !_isFacingRight ? 0 : 180, 0);
        transform.parent.gameObject.transform.rotation = rot;
        _isFacingRight = !_isFacingRight;
    }

    private void FlipAndReset()
    {
        Flip();
        Restart();
    }

    private void Stop()
    {
        _rb.velocity = Vector2.zero;
    }

    private void Restart()
    {
        Stop();
        _shouldMove = false;
        _isGrounded = false;
        _hasWallAhead = false;
        _timeCounter = 0;
        _initialPosition = transform.position;
        _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;
        _rb.gravityScale = 10;
        _maxHeight = 0f;
    }
   
    private void Move()
    {
        float differenceOfPosition = transform.position.x - _initialPosition.x;
        differenceOfPosition =  Mathf.Abs(differenceOfPosition);
        float halfOfDistance = _distance / 2;

        _rb.gravityScale = 1; // Removendo gravidade do calculo controlado
        int direction = _isFacingRight ? 1 : -1;
        _rb.velocity = new Vector2(direction * _forcesXY.x * _speed, _rb.velocity.y);
        
        if (differenceOfPosition < halfOfDistance && differenceOfPosition != 0)
        {
            // 1 é para inverter a curva
            float vY = 1 - _yDynamic.Evaluate(differenceOfPosition / halfOfDistance);
            vY = Mathf.Round(vY * 100f) / 100f; // Arredondando para 2 casas decimais
            
            _rb.velocity = new Vector2(_rb.velocity.x, vY * _forcesXY.y * _speed);
        }
        else if (differenceOfPosition > halfOfDistance)
        {
            if (_maxHeight == 0)
            {
                RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 50f, _groundLayer);
                _maxHeight = ray.distance;
                // Debug.Log(_maxHeight);
            };

            float vY = _yDynamic.Evaluate((differenceOfPosition - halfOfDistance) / halfOfDistance);
            vY *= -1; // Velocidade deve ser negativa

            float vel = vY * _forcesXY.y * (_speed / 2);
            _rb.velocity = new Vector2(_rb.velocity.x, vel);

            // Zerando todas as interações ao tocar o chão
            if (_isGrounded) Restart();
        }
    }

    public void DisableEvents()
    {
        Restart();
        _enableEvents = false;
    }

    public void EnableEvents(int xDirection)
    {
        Restart();
        _isFacingRight = xDirection > 0;
        _shouldMove = true;
        _timeCounter = _cooldown;
        _enableEvents = true;
    }

    public bool IsEventsEnabled()
    {
        return _enableEvents;
    }

    public void UpdateDirection(int xDirection)
    {
        if (!_isGrounded) return;
        if ((xDirection > 0 && !_isFacingRight) || (xDirection < 0 && _isFacingRight))
        {
            Flip();
            Debug.Log("Tá guardado");
        }
    }
}
