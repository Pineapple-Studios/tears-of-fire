using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrack : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerMask;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private float _distanceToCheckPlayer = 10;
    [SerializeField]
    private float _bossHeight = 10;

    const string PREP_DASH_ANIM = "prep_dash";

    private bool _hasPlayer = false;
    private bool _hasPrepared = false;
    private Vector2 _directionToGo;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _bossHeight / 2);
    }

    private void Update()
    {
        if (HasPlayerAhead() && !_hasPlayer)
        {
            _hasPlayer = true;
            if (!_hasPrepared)
            {
                _anim.Play(PREP_DASH_ANIM);
                _hasPrepared = true;
            }
        }
    }

    private bool HasPlayerAhead()
    {
        RaycastHit2D rightHit = Physics2D.CircleCast(
            transform.position,
            _bossHeight / 2,
            Vector2.right,
            _distanceToCheckPlayer,
            _playerMask
        );
        if (rightHit.collider != null) _directionToGo = Vector2.right;

        RaycastHit2D leftHit = Physics2D.CircleCast(
            transform.position,
            _bossHeight / 2,
            Vector2.left,
            _distanceToCheckPlayer,
            _playerMask
        );
        if (leftHit.collider != null) _directionToGo = Vector2.left;

        return rightHit.collider != null || leftHit.collider != null;
    }

    public Vector2 DirectionToGo()
    {
        return _directionToGo;
    }

    public void EnablePlayerPresence()
    {
        _hasPlayer = false;
        _directionToGo = Vector2.zero;
    }
}
