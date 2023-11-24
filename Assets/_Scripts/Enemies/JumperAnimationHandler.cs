using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumperAnimationHandler : MonoBehaviour
{
    [Header("Props")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private LayerMask _groundMask;

    private bool _isGrounded = false;
    private bool _isDown = false;
    private bool _isUp = false;
    private bool _isIdle = false;


    private void Update()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _groundMask);

        if (_rb.velocity.y < 0 && !_isDown)
        {
            Restart();
            _anim.Play("clip_down");
            _isDown = true;
        }
        if (_rb.velocity.y > 0 && !_isUp)
        {
            Restart();
            _anim.Play("clip_up");
            _isUp = true;
        }
        if(_isGrounded && !_isIdle)
        {
            Restart();
            _anim.Play("clip_idle");
            _isIdle = true;
        }
    }

    private void Restart()
    {
        _isDown = false;
        _isUp = false;
        _isIdle = false;
    }
}
