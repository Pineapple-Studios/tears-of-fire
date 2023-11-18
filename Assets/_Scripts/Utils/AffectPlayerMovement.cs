using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class AffectPlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    private bool _enabled = false;
    private Rigidbody2D _target;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _target = collision.gameObject.GetComponent<Rigidbody2D>();
            _enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _target = null;
            _enabled = false;
        }
    }

    private void Update()
    {
        if (!_enabled || !_target) return;
        if (_rb.velocity == Vector2.zero) return;

        _target.velocity += _rb.velocity.y > 0 ? new Vector2(_rb.velocity.x, 0) : _rb.velocity;
        MoveCharacter();
    }

    void MoveCharacter()
    {
        // Inputs
        Vector2 Direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        _target.velocity += new Vector2(190 * Direction.x * Time.deltaTime, 0);
    }
}
