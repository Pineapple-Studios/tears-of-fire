using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class TucanoRexProps : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    [Header("Props")]
    [SerializeField]
    private float _maxLife = 900;
    [SerializeField]
    private float _life = 900;
    [SerializeField]
    private float _damage = 30;

    private PlayerProps _pp;
    private PlayerController _pc;

    private void Start()
    {
        _life = _maxLife;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignorando colisoes com as paredes
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            _pc = collision.gameObject.GetComponent<PlayerController>();
            if (_pp == null) return;

            _pc.SetAttackEnemyPosition(transform.position);
            _pp.TakeDamage(_damage);
        }
    }

    public void ReceiveDamage(float damage)
    {
        _life -= damage;
    }

    public float GetCurrentLife() => _life;
}
