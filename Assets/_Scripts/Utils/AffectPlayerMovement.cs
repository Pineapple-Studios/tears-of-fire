using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class AffectPlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    private bool _enabled = false;
    private Rigidbody2D _target;
    private PlayerController _pc;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _target = collision.gameObject.GetComponent<Rigidbody2D>();
            _pc = collision.gameObject.GetComponent<PlayerController>();
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
        MoveCharacter();
    }


    void MoveCharacter()
    {
        _pc.IncreaseExternalVelocity(new Vector2(_rb.velocity.x, 0));
    }

    public void DisableAffect()
    {
        _enabled = false;
        _target = null;
        _pc = null;
    }
}
