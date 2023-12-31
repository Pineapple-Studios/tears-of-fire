using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private int _damageOnTouch;

    private PlayerProps _pp;
    private PlayerController _pc;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            _pc = collision.gameObject.GetComponent<PlayerController>();
            if (_pp == null) return;

            _pc.SetAttackEnemyPosition(transform.position);
            _pp.TakeDamage(_damageOnTouch);
        }
    }
}
