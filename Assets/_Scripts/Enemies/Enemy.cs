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
    private bool _isDisabledColliders = false;
    private bool _isDamaging = false;
    private float _counter = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp == null) return;

            _isDamaging = true;
            Debug.Log("Damage");
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
            Die();
        }
    }

    private void Die()
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
}
