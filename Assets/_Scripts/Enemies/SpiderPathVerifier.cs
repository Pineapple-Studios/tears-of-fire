using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderPathVerifier : MonoBehaviour
{
    private const string JUMP_CLIP = "isJumping";

    [Header("Casting")]
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Props")]
    [SerializeField]
    private float _jumpDistance = 3f;
    [Tooltip("Velocidade em que a spider viaja o previamente definido `Jump Distance`")]
    [SerializeField]
    private float _jumpVelocity = 0.2f;
    [SerializeField]
    private float _gravityScale = 5;

    private Animator _anim;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private SpiderPlayerTrack _spt;
    private Enemy _en;
    private bool _isOnGround = true;
    private bool _isLookingRight = false;
    private Vector3 _np = Vector3.zero;

    private bool _isNearGround = true;
    private bool _isFowardBlocked = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _spt = gameObject.GetComponent<SpiderPlayerTrack>();
        _en = gameObject.GetComponent<Enemy>();
    }

    private void Update()
    {
        if(_en.GetCurrentLife() <= 0)
        {
            _rb.gravityScale = 0;
            return;
        }

        if (IsJumping() && !_isOnGround) UpdateSpiderPosition();

        _isNearGround = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _groundLayer);
        _isFowardBlocked = Physics2D.Raycast(
            transform.position + new Vector3(0, _sr.bounds.size.y / 2,0), 
            _isLookingRight ? Vector2.right : Vector2.left, 
            _jumpDistance + 1f, 
            _groundLayer
        );

        if (_isNearGround) _rb.velocity = new Vector2(_rb.velocity.x, 0);
        
        if (_isFowardBlocked) Flip();
        if (_spt.PlayerDirection().x > 0 && !_isLookingRight) Flip();
        if (_spt.PlayerDirection().x < 0 && _isLookingRight) Flip();
    }

    private void UpdateSpiderPosition()
    {
        transform.position = Vector3.Lerp(transform.position, _np, _jumpVelocity * Time.deltaTime);
    }

    private void Flip()
    {
        _isLookingRight = !_isLookingRight;
        UpdateSpritePosition();
    }

    private void UpdateSpritePosition()
    {
        _sr.flipX = _isLookingRight;
    }

    public bool IsJumping() => _anim.GetBool("isJumping");
    public bool IsLookingRight() => _isLookingRight;

    public void Jump()
    {
        _anim.SetBool(JUMP_CLIP, true);
    }

    public void ResetSpider()
    {
        _anim.Update(0f);
        _anim.Rebind();
    }

    public void ToggleGravityScale()
    {
        if (_rb.gravityScale == 0) _rb.gravityScale = _gravityScale;
        else _rb.gravityScale = 0;
    }

    public void ToggleOnGround()
    {
        _isOnGround = !_isOnGround;
    }

    public void SetNewPosition()
    {
        if (_isLookingRight)
        {
            _np = transform.position + new Vector3(_jumpDistance, 0, 0);
        }
        else
        {
            _np = transform.position - new Vector3(_jumpDistance, 0, 0);
        }
    }
}
