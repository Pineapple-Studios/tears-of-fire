using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPlayerTrack : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private float _distanceToTrackPlayer = 7;
    [SerializeField]
    private LayerMask _groundLayer;

    private GameObject _player;
    private SpriteRenderer _sr;
    private SpiderPathVerifier _spv;
    private SpiderCicleHandler _sch;
    private float _targetDistance;
    private bool _isUnblockedPath = false;
    private Vector3 _playerDiretion = Vector3.zero;

    private void OnDrawGizmos()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        Gizmos.DrawWireSphere(
            transform.position + new Vector3(0, _sr.bounds.size.y / 2, 0), 
            _distanceToTrackPlayer
        );
    }

    private void OnBecameVisible()
    {
        ToggleFollowing();
    }

    private void OnBecameInvisible()
    {
        ToggleFollowing();
        StopMoviment();
    }

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _spv = gameObject.GetComponent<SpiderPathVerifier>();
        _sch = gameObject.GetComponent<SpiderCicleHandler>();
    }

    private void Update()
    {
        if (!_player) return;

        CalcDistance();

        if (_targetDistance <= _distanceToTrackPlayer) CheckPath();

        if (_isUnblockedPath && !_spv.IsJumping() && _sch.CanJump()) TriggerJump();
    }

    private void CheckPath()
    {
        Vector3 origin = transform.position + new Vector3(0, _sr.bounds.size.y / 2, 0);
        Vector3 playerDirection = _player.transform.position + new Vector3(0, _player.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2, 0);
        Vector3 direction = playerDirection - origin;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, _targetDistance, _groundLayer);

        _isUnblockedPath = hit.collider == null;

        _playerDiretion = direction.normalized;
    }

    private void TriggerJump()
    {
        _spv.Jump();
    }

    private void CalcDistance()
    {
        _targetDistance = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void ToggleFollowing()
    {
        if (_player == null)
            _player = FindFirstObjectByType<PlayerController>().gameObject;
        else
            _player = null;
    }

    public void StopMoviment()
    {
        _isUnblockedPath = false;
    }

    public Vector3 PlayerDirection() => _playerDiretion;
}
