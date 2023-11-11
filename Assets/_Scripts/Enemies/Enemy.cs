using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _damageOnTouch = 35f;
    [SerializeField]
    private float _life = 35f;
    [SerializeField]
    private float _damageColldown = 1f;
    [SerializeField]
    private Collider2D _col;

    private PlayerProps _pp;
    private PlayerController _pc;
    private Animator _ac;
    private IWalkStart _ws;
    private bool _isDisabledColliders = false;
    private bool _isDamaging = false;
    private float _counter = 0f;

    // Nome dos clipes de animação
    private const string IDLE = "clip_idle";
    private const string WALK = "clip_walk";
    private const string ATTACK = "clip_attack";
    private const string HIT = "clip_hit";
    private const string DEATH = "clip_death";

    private void Start()
    {
        if (gameObject.GetComponent<Animator>() != null) _ac = gameObject.GetComponent<Animator>();
        else _ac = transform.parent.gameObject.GetComponentInChildren<Animator>();

        if (GetComponent<IWalkStart>() != null) _ws = GetComponent<IWalkStart>();
        else _ws = transform.parent.gameObject.GetComponentInChildren<IWalkStart>();

        _ac.Play(IDLE);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            _pc = collision.gameObject.GetComponent<PlayerController>();
            if (_pp == null) return;

            _isDamaging = true;
            _ac.Play(ATTACK);
            
            _pc.SetAttackEnemyPosition(transform.position);
            _pp.TakeDamage(_damageOnTouch);

        }

        // Ignorando colisoes entre elementos com a mesma tag
        if (((1 << collision.gameObject.layer) & gameObject.layer) != 0)
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }

    private void Update()
    {
        if (_ws != null) _ws.OnStartWalking();
        if (_isDamaging && _counter < _damageColldown) _counter += Time.deltaTime;
        else
        {
            _isDamaging = false;
            _counter = 0f;
        }

        if (_pp == null || _col == null) return;

        // Desabilitando colisor do inimigo após levar um dano
        if (_pp.IsTakingDamage && _isDisabledColliders == false)
        {
            // _col.enabled = false;
            _isDisabledColliders = true;

            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(_pp.RecallColliderTime);
        _col.enabled = true;
        _isDisabledColliders = false;
    }

    public void TakeDamage(float hit)
    {
        _life -= hit;

        if (_life <= 0)
        {
            _ac.Play(DEATH);
        }
        else
        {
            _ac.Play(HIT);
        }
    }

    public void Die()
    {
        // Start damaged animation

        // Going to main parent
        GameObject tmpObj = gameObject;
        while (tmpObj.transform.parent != null)
        {
            tmpObj = tmpObj.transform.parent.gameObject;
        }

        // Remove enemy
        Destroy(tmpObj);
    }

    public float GetDamagePoints()
    {
        return _damageOnTouch;
    }

    public void StartWalk()
    {
        _ac.Play(WALK);
    }

    public float GetCurrentLife()
    {
        return _life;
    }
}
