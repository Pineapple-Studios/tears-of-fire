using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BossMoviment : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private Transform _leftLimit;
    [SerializeField]
    private Transform _rightLimit;
    [SerializeField]
    private LayerMask _wallLayer;
    [SerializeField]
    private GameObject _goPlayer;
    [SerializeField]
    private SpriteRenderer _sprite;

    [Header("BossProps")]
    [Header("Tear One")]
    [Tooltip("Tempo de intervalo entre os dashes enquanto a vida do chefe estiver entre 600-400")]
    [SerializeField]
    private float _cooldownTearOne = 3f;
    [SerializeField]
    private float _dashMultiplayerOne = 50f;
    [Header("Tear Two")]
    [Tooltip("Tempo de intervalo entre os dashes enquanto a vida do chefe estiver entre 399-300")]
    [SerializeField]
    private float _cooldownTearTwo = 2f;
    [SerializeField]
    private float _dashMultiplayerTwo = 50f;
    [Header("Tear Three")]
    [Tooltip("Tempo de intervalo entre os pulos enquanto a vida do chefe estiver entre 299-200")]
    [SerializeField]
    private float _cooldownTearThree = 2f;
    [SerializeField]
    private float _dashMultiplayerThree = 50f;
    [SerializeField]
    private float _jumpHeightThree = 7f;
    [Header("Tear Four")]
    [Tooltip("Tempo de intervalo entre os pulos enquanto a vida do chefe estiver entre 299-200")]
    [SerializeField]
    private float _cooldownTearFour = 2f;
    [SerializeField]
    private float _dashMultiplayerFour = 50f;
    [SerializeField]
    private float _jumpHeightFour = 7f;
    [SerializeField]
    private GameObject _leftPlatform;
    [SerializeField]
    private GameObject _rightPlatform;

    private Rigidbody2D _rb;
    private BossProps _bp;

    // New
    private enum xDir { LEFT = -1, RIGHT = 1 };
    private Transform _currentTransform;
    private float _dashMultiplyer = 0f;
    private float _jumpHeight = 0f;
    private float _maxTime = 0f;
    private float _timer = 0f;
    private xDir _xDir = xDir.LEFT;
    private bool _isStopped = false;
    private float _bossHeight = 0f;
    private bool _isDiving;
    private Vector3 _divingPos;
    private Transform _divingTarget;
    private bool _setupFour = false;
    private bool _goingToLeft = false;

    // Exposed props
    private bool _mustDropBomb = false;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _bp = GetComponent<BossProps>();
        _bossHeight = _sprite.bounds.size.y;
        _divingTarget = new GameObject().transform;

        _maxTime = _cooldownTearOne;
        _currentTransform = transform.parent.transform;

        // Removendo parentesco dos pontos de controle
        _leftLimit.transform.parent = null;
        _rightLimit.transform.parent = null;
        _leftPlatform.transform.parent = null;
        _rightPlatform.transform.parent = null;

        // Desativando cenário do último Tear
        _leftPlatform.SetActive(false);
        _rightPlatform.SetActive(false);
    }

    private void Update()
    {
        switch (_bp.GetCurrentTear())
        {
            case Tears.faseOne:
                TearSetup(_cooldownTearOne, _dashMultiplayerOne);
                FirstMoviment();
                break;
            case Tears.faseTwo:
                TearSetup(_cooldownTearTwo, _dashMultiplayerTwo);
                FirstMoviment();
                break;
            case Tears.faseThree:
                TearSetup(_cooldownTearThree, _dashMultiplayerThree);
                if (_divingTarget.position != _goPlayer.transform.position) _divingTarget.position = _goPlayer.transform.position;
                if (_jumpHeight != _jumpHeightThree) _jumpHeight = _jumpHeightThree;
                SecondMoviment();
                break;
            case Tears.faseFour:
                TearSetup(_cooldownTearFour, _dashMultiplayerFour);
                if (_jumpHeight != _jumpHeightFour) _jumpHeight = _jumpHeightFour;
                ThirdMoviment();
                break;
            default:
                _rb.velocity = Vector2.zero;
                break;
        }

        _mustDropBomb = _isDiving && !IsOnGround() && _setupFour;

        if (!_isStopped) InterupMoviment();
    }

    private void ThirdMoviment()
    {
        // Anchors
        Transform _lTrans = _leftPlatform.GetComponentInChildren<AnchorPoint>().gameObject.transform;
        Transform _rTrans = _rightPlatform.GetComponentInChildren<AnchorPoint>().gameObject.transform;
        // Offset
        Vector3 offSet = new Vector3(0, _bossHeight / 2, 0);

        if (!_setupFour)
        {
            _isDiving = false;

            // Ativando cenário do último Tear
            _leftPlatform.SetActive(true);
            _rightPlatform.SetActive(true);
            // Desativando os sprites
            _sprite.gameObject.SetActive(false);

            // Definindo posição inicial do boss para o tear 4
            _currentTransform.position = _lTrans.position;
            _currentTransform.position += offSet;

            // TODO: Tocar animação aqui!!

            _sprite.gameObject.SetActive(true);

            _divingTarget.position = _rTrans.position;
            _divingTarget.position += offSet;
            
            _goingToLeft = false;
            _setupFour = true;
        }

        SecondMoviment();
    }

    /// <summary>
    /// Define o valor de cooldown e velicidadeDoDash para cada fase do boss
    /// </summary>
    /// <param name="cooldown">Cooldown a ser utilizado</param>
    /// <param name="dashMultiplyer">Valocidade do movimento</param>
    private void TearSetup(float cooldown, float dashMultiplyer)
    {
        if (_maxTime != cooldown) _maxTime = cooldown;
        if (_dashMultiplyer != dashMultiplyer) _dashMultiplyer = dashMultiplyer;
    }

    /// <summary>
    /// Interrompe o movimento do boss caso encontre uma parede nas laterais
    /// </summary>
    private void InterupMoviment()
    {
        RaycastHit2D rightHit = Physics2D.Raycast(_currentTransform.position, Vector2.right, 4f, _wallLayer);
        // Debug.DrawRay(_currentTransform.position, Vector2.right * 4f, Color.green);
        RaycastHit2D leftHit = Physics2D.Raycast(_currentTransform.position, Vector2.left, 4f, _wallLayer);
        // Debug.DrawRay(_currentTransform.position, Vector2.left * 4f, Color.green);

        // Debug.Log($"Left: {leftHit.collider}");
        // Debug.Log($"Right: {rightHit.collider}");

        if (rightHit.collider != null || leftHit.collider != null)
        {
            _rb.velocity = Vector2.zero;
            _isStopped = true;
        }
    }

    /// <summary>
    /// Salto com mergulho do boss
    /// </summary>
    private void SecondMoviment()
    {
        if (_isDiving && !IsOnGround())
        {
            GoToPoint(_divingPos);
            return;
        }

        if (IsOnGround() && _isDiving && IsCoolDownFinished())
        {
            _isDiving = false;
            _isStopped = false;
            _timer = 0f;

            // Apenas chamado no quarto movimento
            if (_setupFour) InverseTarget();
        } 

        //Debug.DrawRay(
        //    _currentTransform.position + new Vector3(0, -(_bossHeight / 2), 0), 
        //    Vector2.down * _jumpHeight, 
        //    Color.green
        //);

        if (RisingCast() && !_isDiving)
        {
            RisingUp();
        }
        else
        {
            if (_isStopped) _isStopped = false;
            _rb.velocity = Vector2.zero;
            _divingPos = _divingTarget.position - _currentTransform.position;
            _isDiving = true;
        }
    }

    /// <summary>
    /// Movimento lateral do personagem
    /// </summary>
    private void FirstMoviment()
    {
        if (_currentTransform.position.x >= _leftLimit.position.x && _xDir == xDir.LEFT)
        {
            DashingToLeftLimit();
            return;
        }

        if (_currentTransform.position.x <= _rightLimit.position.x && _xDir == xDir.RIGHT)
        {
            DashingToRightLimit();
            return;
        }
        
        if (IsCoolDownFinished())
        {
            // Inverse direction
            _xDir = _xDir == xDir.RIGHT ? xDir.LEFT : xDir.RIGHT;

            _timer = 0f;
            _isStopped = false;
        }
    }

    /// <summary>
    /// Controla a velocidade de um corpo orientando-o para esquerda
    /// </summary>
    private void DashingToLeftLimit()
    {
        _rb.velocity = Vector2.left * _dashMultiplyer;
    }

    /// <summary>
    /// Controla a velocidade de um corpo o orientando para direita
    /// </summary>
    private void DashingToRightLimit()
    {
        _rb.velocity = Vector2.right * _dashMultiplyer;
    }

    /// <summary>
    /// Controla a velocidade de um corpo para subida
    /// </summary>
    private void RisingUp()
    {
        _rb.velocity = Vector2.up * _dashMultiplyer;
    }

    /// <summary>
    /// Define a velocidade de um body direcionando o a um ponto
    /// </summary>
    /// <param name="point">Ponto destino</param>
    private void GoToPoint(Vector3 point)
    {
        _rb.velocity = point.normalized * _dashMultiplyer;
    }

    /// <summary>
    /// Controlador se para os momentos de descanso do Boss
    /// </summary>
    /// <returns>Caso o cooldown estiver finalizado retorna true</returns>
    private bool IsCoolDownFinished()
    {
        _rb.velocity = Vector2.zero;
        _timer += Time.deltaTime;

        return _timer >= _maxTime; 
    }

    /// <summary>
    /// Verifica se existe um chão abaixo de você
    /// </summary>
    private bool IsOnGround()
    {
        RaycastHit2D onGroundHit = Physics2D.Raycast(
            _currentTransform.position,
            Vector2.down,
            (_bossHeight / 2) + 0.4f,
            _wallLayer
        );

        return onGroundHit.collider != null;
    }

    /// <summary>
    /// Verificador da altura do pulo
    /// </summary>
    private bool RisingCast()
    {
        RaycastHit2D bottomHit = Physics2D.Raycast(
            _currentTransform.position,
            Vector2.down,
            (_bossHeight / 2) + _jumpHeight,
            _wallLayer
        );

        return bottomHit.collider != null;
    }
    
    /// <summary>
    /// Inverte o target do movimento final
    /// </summary>
    private void InverseTarget()
    {
        // Anchors
        Transform _lTrans = _leftPlatform.GetComponentInChildren<AnchorPoint>().gameObject.transform;
        Transform _rTrans = _rightPlatform.GetComponentInChildren<AnchorPoint>().gameObject.transform;
        // Offset
        Vector3 offSet = new Vector3(0, _bossHeight / 2, 0);

        if (!_goingToLeft)
        {
            _divingTarget.position = _lTrans.position;
            _divingTarget.position += offSet;
            _goingToLeft = true;
        }
        else
        {
            _divingTarget.position = _rTrans.position;
            _divingTarget.position += offSet;
            _goingToLeft = false;
        }
    }

    /// <summary>
    /// Permite que identifiquemos quando devemos ou não derrubar as bombas no cenário
    /// </summary>
    public bool GetMustDropBombState()
    {
        return _mustDropBomb;
    }
}

