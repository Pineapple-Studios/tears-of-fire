using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public static Action onPlayerDashing;

    [Header("Física")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Propriedades da mecânica")]
    [SerializeField]
    private float _dashDuration = 1f;
    [SerializeField]
    private float _dashRecall = 1f;
    [SerializeField]
    private float _dashMultiplyer = 2f;

    public bool IsDashed = false;
    private bool canDash = true;
    private PlayerController _pc;
    private float _prevGravityScale;
    private Vector2 _prevVelocity;

    private void Start()
    {
        canDash = true;
        _pc = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            // Debug.Log("Dash");
            StartCoroutine(Dash());
        }

        if (IsDashed)
        {
            AvoidPhysicsBroken();
        }
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

    private IEnumerator Dash()
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
        yield return new WaitForSeconds(_dashRecall);
        canDash = true;
    }
}
