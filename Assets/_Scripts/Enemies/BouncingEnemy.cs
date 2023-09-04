using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Transform _reference;
    [SerializeField]
    private Rigidbody2D _rb;

    [Header("Settings")]
    [SerializeField]
    private float _distanceToGround = 0.2f;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _distanceToStartPoint = 0.2f;

    private Vector3 _initialPosition;
    private bool _willHitTheGround;
    private bool _willHitTheStartPoint;
    private Vector3 _bottomElementPoint;
    private bool _hasVelocityControllerEnabled = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        _bottomElementPoint = _reference.position + new Vector3(transform.localPosition.x, -_spriteRenderer.size.y / 2, transform.localPosition.z);
        Gizmos.DrawLine(_bottomElementPoint, _bottomElementPoint + Vector3.down * _distanceToGround);
    }

    private void Start()
    {
        _bottomElementPoint = _reference.position + new Vector3(transform.localPosition.x, -_spriteRenderer.size.y / 2, transform.localPosition.z);
        _initialPosition =  new Vector3(_reference.position.x, _reference.position.y, _reference.position.z);
    }

    private void FixedUpdate()
    {
        _willHitTheGround =
            Physics2D.Raycast(_bottomElementPoint, Vector2.down, _distanceToGround, _groundLayer);
        _willHitTheStartPoint = Vector3.Distance(_initialPosition, _reference.position) < _distanceToStartPoint;

        if (!_hasVelocityControllerEnabled && _willHitTheGround) _hasVelocityControllerEnabled = true;
        if (_willHitTheStartPoint && _hasVelocityControllerEnabled)
        {
            _hasVelocityControllerEnabled = false;
            _rb.velocity = Vector2.zero;
        }
    }
}
