using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBullet : MonoBehaviour
{
    public float Speed = 10f;
    public float Lifetime = 5f;

    private Vector3 _direction = Vector3.zero;
    private LayerMask _targetLayer;
    private float _damage;
    private float _timer;

    private void Start()
    {
        transform.parent = null;
    }

    void Update()
    {
        if (_direction == Vector3.zero) return;

        _timer += Time.deltaTime;
        if (_timer > Lifetime)
        {
            Destroy(gameObject);
        }

        transform.position += _direction * Speed * Time.deltaTime;
    }

    public void SetTarget(Transform target, LayerMask mask, float damage)
    {
        _targetLayer = mask;
        _damage = damage;
        _direction = (target.position - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _targetLayer) != 0))
        {
            Debug.Log(collision.gameObject.name);
            PlayerProps _pp = collision.gameObject.GetComponent<PlayerProps>();
            if (_pp != null) _pp.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }

}
