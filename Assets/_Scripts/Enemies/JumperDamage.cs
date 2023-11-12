using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperDamage : MonoBehaviour
{
    [SerializeField]
    private float _damageOnTouch = 35f;

    private PlayerProps _pp;
    private PlayerController _pc;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("JumperDamage");
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            _pc = collision.gameObject.GetComponent<PlayerController>();
            if (_pp == null) return;

            _pc.SetAttackEnemyPosition(transform.position);
            _pp.TakeDamage(_damageOnTouch);
        }

        // Ignorando colisoes entre elementos com a mesma tag
        if (((1 << collision.gameObject.layer) & gameObject.layer) != 0)
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}
