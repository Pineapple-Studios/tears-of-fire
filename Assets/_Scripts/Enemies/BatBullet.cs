using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBullet : MonoBehaviour
{
    public float Speed = 10f;
    public float Lifetime = 5f;

    [SerializeField]
    private LayerMask _instantDestroyLayer;

    private Vector3 _direction = Vector3.zero;
    private LayerMask _targetLayer;
    private float _damage;
    private float _timer;
    private bool _mustDestroy = false;

    private void Start()
    {
        transform.parent = null;
    }

    void Update()
    {
        // Definindo direção do projétil
        if (_direction == Vector3.zero) return;

        // Identifica se está colidindo comparede em alguma direção
        _mustDestroy = 
            Physics2D.Raycast(transform.position, Vector2.up, 0.1f, _instantDestroyLayer) ||
            Physics2D.Raycast(transform.position, Vector2.left, 0.1f, _instantDestroyLayer) ||
            Physics2D.Raycast(transform.position, Vector2.right, 0.1f, _instantDestroyLayer) ||
            Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _instantDestroyLayer);

        _timer += Time.deltaTime;
        if (_timer > Lifetime || _mustDestroy)
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
            PlayerProps _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp != null) _pp.TakeDamageWhithoutKnockback(_damage);

            Destroy(gameObject);
        }
    }

}
