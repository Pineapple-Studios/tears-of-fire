using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Jumper : MonoBehaviour
{
    [Header("Engine props")]
    [SerializeField]
    private float _horizontalForce = 3f;
    [SerializeField]
    private float _jumpForce = 3f;
    [SerializeField]
    private float _jumpUpTime = 3f;
    [SerializeField]
    private float _delayToStopMoviment = 1f;

    [Header("GameObject props")]
    [SerializeField]
    private Rigidbody2D _rb;

    [Header("Casting props")]
    [SerializeField]
    private float _castRadius = 5f;
    [SerializeField]
    private LayerMask _playerMask;
    [SerializeField]
    private float _distanceToGround = 1f;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private LayerMask _wallLayer;
    [SerializeField]
    private float _wallDistance = 2.5f;
    [SerializeField]
    private Vector3 _offSet = new Vector3();

    private CircleCollider2D _circleCollider;
    private bool _isGrounded = false;
    private bool _hasWallAhead = false;
    private bool _isJumping = false;

    private int _xDir = 1;
    private bool _identifyPlayer = false;
    private Vector3 _foward2D;
    private float _jumpTimeCounter = 0f;


    private float _timerToChangeDir = 0f; 
    private float _timerToStop = 0f;
    private Transform _parent = null;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _castRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector3.down * _distanceToGround + transform.position);
        Gizmos.color = Color.blue;
        _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position + _offSet, _foward2D * _wallDistance + (transform.position + _offSet));
    }

    private void Awake()
    {
        _circleCollider = transform.AddComponent<CircleCollider2D>();
        _circleCollider.radius = _castRadius;
        _circleCollider.isTrigger = true;
        _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;
    }


    private void Start()
    {
        _parent = transform.parent.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerMask) != 0 && _isGrounded)
        {
            // Definindo direção do movimento horizontal
            float direction = (transform.position - collision.gameObject.transform.position).normalized.x;
            _xDir = direction < 0 ? 1 : -1;
            // Definindo rotação de acordo com o elemento colidido
            transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, direction < 0 ? 0 : 180, 0);
            // Ativando movimento
            _identifyPlayer = _hasWallAhead ? false : true;
            _timerToStop = 0f;
        }
    }


    private void Update()
    {
        if (_parent.GetComponentInChildren<Enemy>().GetCurrentLife() <= 0) return;

        if (_foward2D != Quaternion.AngleAxis(90, Vector3.up) * transform.forward)
            _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;

        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position + _offSet,
            2f,
            _foward2D,
            _castRadius,
            _playerMask
        );

        if (hit.collider == null && _timerToStop < _delayToStopMoviment)
        {
            _timerToStop += Time.deltaTime;
        }

        if (_timerToStop > _delayToStopMoviment)
        {
            _identifyPlayer = false;
        }

        // Iniciando pulo quando no chão
        _isGrounded =
            Physics2D.Raycast(transform.position, Vector2.down, _distanceToGround, _groundLayer);

        // Zerando todas as interações ao tocar o chão
        if (_isGrounded)
        {
            ResetBody();
        }
        else
        {
            // Limitando velocidade de queda
            if (_rb.velocity.y < -40f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -40f);
            }
        }

        
        // Trocando a direção com controle de tempo
        _hasWallAhead = Physics2D.Raycast(transform.position + _offSet, _foward2D, _wallDistance, _wallLayer);

        if (_hasWallAhead && _timerToChangeDir >= 2)
        {
            Flip();
            _xDir *= -1;
            _timerToChangeDir = 0;
        }

        if (_timerToChangeDir < 2) _timerToChangeDir += Time.deltaTime;


        if (_rb.velocity.y > 0)
        {
            _parent.GetComponentInChildren<Animator>().Play("clip_up");
        }
        if (_rb.velocity.y < 0)
        {
            _parent.GetComponentInChildren<Animator>().Play("clip_down");
        }
    }

    private void FixedUpdate()
    {
        if (_identifyPlayer) AddMoviment();
    }

    
    private void AddMoviment()
    {
        _rb.velocity = new Vector2(_xDir * _horizontalForce * Time.fixedDeltaTime, _rb.velocity.y);
        
        if (_jumpTimeCounter < _jumpUpTime && !_isJumping)
        {
            Jump();
        }
        else
        {
            _rb.velocity = new Vector2(_xDir * _horizontalForce * Time.fixedDeltaTime, 0f);
            _jumpTimeCounter = 0;
            _isJumping = true;
            _identifyPlayer = false;
        }
        
    }

    /// <summary>
    /// Executa o pulo do personagem
    /// </summary>
    void Jump()
    {
        if (!_isGrounded) return;

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        bool IsFacingRight = _xDir == 1;
        Quaternion rot = Quaternion.Euler(0, !IsFacingRight ? 0 : 180, 0);

        transform.parent.gameObject.transform.rotation = rot;
    }

    private void ResetBody()
    {
        _isJumping = false;
        _jumpTimeCounter = 0;
        _rb.velocity = Vector2.zero;
    }
}
