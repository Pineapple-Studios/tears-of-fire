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
    private float _impulseForce = 5f;
    [SerializeField]
    private float _angleToImpulse = 45f;
    [SerializeField]
    private float _delayToNextJump = 1f;

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

    private CircleCollider2D _circleCollider;
    private bool _isGrounded;
    private bool _isPrevoiusOnGround;
    private bool _isImpulsed = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _castRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector3.down * _distanceToGround + transform.position);
    }

    private void Awake()
    {
        _circleCollider = transform.AddComponent<CircleCollider2D>();
        _circleCollider.radius = _castRadius;
        _circleCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerMask) != 0)
        {
            float direction = (transform.position - collision.gameObject.transform.position).normalized.x;
            direction = direction < 0 ? _angleToImpulse : -_angleToImpulse;
            if (_isGrounded && !_isImpulsed)
            {
                _isImpulsed = true;
                AddForceAtAngle(_impulseForce, direction);
                StartCoroutine(EnableJump());
            }
        }
    }

    private IEnumerator EnableJump()
    {
        yield return new WaitForSeconds(_delayToNextJump);
        _isImpulsed = false;
        _rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        _isGrounded =
            Physics2D.Raycast(transform.position, Vector2.down, _distanceToGround, _groundLayer);

        if (!_isPrevoiusOnGround && _isGrounded) _rb.velocity = Vector2.zero;

        _isPrevoiusOnGround = _isGrounded;
    }

    public void AddForceAtAngle(float force, float angle)
    {
        angle *= Mathf.Deg2Rad;
        float xComponent = Mathf.Sin(angle) * force;
        float yComponent = Mathf.Cos(angle) * force;
        Vector2 forceApplied = new Vector2(xComponent, yComponent);

        _rb.AddForce(forceApplied, ForceMode2D.Impulse);
    }
}
