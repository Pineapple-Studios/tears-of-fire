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
    private Collider2D _col;

    private PlayerProps _pp;
    private bool _isDisabledColliders = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp == null) return;
                
            _pp.TakeDamage(_damageOnTouch);
        }
    }

    private void Update()
    {
        if (_pp == null || _col == null) return;

        // Desabilitando colisor do inimigo após levar um dano
        if (_pp.IsTakingDamage && _isDisabledColliders == false)
        {
            _col.enabled = false;
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
        Debug.Log("Damaged me");
        _life -= hit;

        if (_life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Start damaged animation

        // Remove enemy
        Destroy(gameObject);
    }
    public float GetDamagePoints()
    {
        return _damageOnTouch;
    }
}
