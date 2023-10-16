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
    private float _verticalForce = 5f;
    [SerializeField]
    private AnimationCurve _flow;
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

    private CircleCollider2D _circleCollider;
    private bool _isGrounded = false;
    private bool _hasWallAhead = false;

    private int _xDir = 1;
    private int _yDir = 1;
    private bool _identifyPlayer = false;
    private float _timer = 0f;
    private double _prevEval = 0;
    private Vector3 _foward2D;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _castRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector3.down * _distanceToGround + transform.position);
        Gizmos.color = Color.blue;
        _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position, _foward2D * _wallDistance + transform.position);
    }

    private void Awake()
    {
        _circleCollider = transform.AddComponent<CircleCollider2D>();
        _circleCollider.radius = _castRadius;
        _circleCollider.isTrigger = true;
        _foward2D = Quaternion.AngleAxis(90, Vector3.up) * transform.forward;
    }

    private void OnTriggerStay2D(Collider2D collision)
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerMask) != 0)
        {
            StartCoroutine(StopMoviment());
        }
    }

    private IEnumerator StopMoviment()
    {
        // Delay para encerrar o movimento
        yield return new WaitForSeconds(_delayToStopMoviment);
        _identifyPlayer = false;
    }

    private void Update()
    {
        _isGrounded =
            Physics2D.Raycast(transform.position, Vector2.down, _distanceToGround, _groundLayer);

        // Zerando todas as interações ao tocar o chão
        if (_isGrounded)
        {
            ResetBody();
        }

        
    }

    private void FixedUpdate()
    {
        _hasWallAhead =
           Physics2D.Raycast(transform.position, _foward2D, _wallDistance, _wallLayer);
        
        if (_hasWallAhead)
        {
            Flip();
            _xDir *= -1;
        }

        if (_identifyPlayer || (!_identifyPlayer && !_isGrounded)) MovimentByAnimation();
    }

    private void Flip()
    {
        bool IsFacingRight = _xDir == 1;
        Quaternion rot = Quaternion.Euler(0, !IsFacingRight ? 0 : 180, 0);

        transform.parent.gameObject.transform.rotation = rot;
    }

    private void ResetBody()
    {
        _rb.velocity = Vector2.zero;
        _timer = 0;
        _yDir = 1;
        _prevEval = 0;
    }

    private void MovimentByAnimation()
    {
        _timer += Time.fixedDeltaTime;
        float eval = _flow.Evaluate(_timer);
        double dEval = Math.Round(eval, 3);

        float vForce = (1 - eval) * _verticalForce;
        float hForce = _timer == 0 ? 0 : _horizontalForce;

        // Controlando grandes quedas
        if (!_isGrounded && _yDir == -1) {
            float finalVelocity = _rb.velocity.y < _yDir * vForce ? _rb.velocity.y : _yDir * vForce;
            _rb.velocity = new Vector2(_xDir * hForce, finalVelocity);
            return; 
        }
        else
        {
            _rb.velocity = new Vector2(_xDir * hForce, _yDir * vForce);
        }

        if (((1 - dEval) == 0 || (1 - dEval) == 1) && dEval != _prevEval)
        {
            _yDir *= -1;
            _prevEval = dEval;
        }
    }
}
