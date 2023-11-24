using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public static Action onPlayerDashing;

    [Header("Actions")]
    [SerializeField]
    InputActionAsset Actions;

    [Header("F�sica")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Propriedades da mec�nica")]
    [SerializeField]
    private float _dashDuration = 1f;
    [SerializeField]
    private float _dashRecall = 1f;
    [SerializeField]
    private float _dashRecallOnGround = 0.3f;
    [SerializeField]
    private float _dashMultiplyer = 2f;

    public bool IsDashed = false;

    private bool canDash = true;
    private PlayerController _pc;
    private float _prevGravityScale;
    private Vector2 _prevVelocity;
    private bool _isOnGround = false;

    private void Awake()
    {
        Actions.FindActionMap("Gameplay").FindAction("PowerUp").performed += OnDash;
        Actions.FindActionMap("Gameplay").FindAction("PowerUp").canceled += AvoidPhysicsBroken;
    }

    private void Start()
    {
        canDash = true;
        _pc = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        PlayerProps.onPlayerDead += Respawn;
        PlayerController.onPlayerGround += SetIsGround;
        PlayerController.onPlayerJumping += SetIsJumping;
        Actions.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDead -= Respawn;
        PlayerController.onPlayerGround -= SetIsGround;
        PlayerController.onPlayerJumping -= SetIsJumping;
        Actions.FindActionMap("Gameplay").Disable();
    }

    void Update()
    {
        if (IsDashed) AvoidPhysicsBroken();
    }

    private void Respawn(GameObject obj)
    {
        if (!this.enabled) return;

        canDash = true;
        IsDashed = false;
        _rb.gravityScale = GetComponentInParent<PlayerController>().GravityScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 size = GetComponentInParent<Collider2D>().bounds.size;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * size.x);
    }

    private void AvoidPhysicsBroken()
    {
        Vector3 size = GetComponentInParent<Collider2D>().bounds.size;
        if (Physics2D.Raycast(transform.position, Vector2.right, size.x, _groundLayer))
        {
            StartCoroutine(StopDash());
        };
    }

    private void AvoidPhysicsBroken(InputAction.CallbackContext context) { AvoidPhysicsBroken(); }

    private IEnumerator Dash(InputAction.CallbackContext context)
    {
        float direction = _rb.velocity.x;
        
        // Habilitando dash sem apertar pros lados
        if (direction == 0 && _pc.IsFacingRight) direction = _pc.MoveSpeed;
        if (direction == 0 && !_pc.IsFacingRight) direction = _pc.MoveSpeed * - 1;

        onPlayerDashing();
        canDash = false;
        IsDashed = true;
        _prevVelocity = _rb.velocity;
        _rb.velocity = new Vector2(direction * _dashMultiplyer, 0);
        _prevGravityScale = _rb.gravityScale;
        _rb.gravityScale = 0;
        yield return new WaitForSeconds(_dashDuration);
        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        _rb.velocity = new Vector2(_prevVelocity.x, 0);
        _rb.gravityScale = _prevGravityScale;
        IsDashed = false;
        yield return new WaitForSeconds(_isOnGround ? _dashRecallOnGround : _dashRecall);
        canDash = true;
    }

    private void SetIsGround()
    {
        _isOnGround = true;
    }

    private void SetIsJumping()
    {
        _isOnGround = false;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash) // Verifica se o Dash está disponível
        {
            StartCoroutine(Dash(context));
        }
    }

}
