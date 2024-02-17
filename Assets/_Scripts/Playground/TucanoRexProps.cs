using System;
using UnityEngine;

public class TucanoRexProps : MonoBehaviour
{
    public static Action<GameObject> onTucanoRexDead;

    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    [Header("Setup")]
    [SerializeField]
    private Animator _anim;

    [Header("Props")]
    [SerializeField]
    private float _maxLife = 900;
    [SerializeField]
    private float _life = 900;
    [SerializeField]
    private float _damage = 30;
    [SerializeField]
    private float _colldownDamage = 2;

    private PlayerProps _pp;
    private PlayerController _pc;
    private bool _isCooldownDamage = false;
    private float _cooldownTimer = 0f;
    private bool _isDead = false;

    private void Start()
    {
        _life = _maxLife;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignorando colisoes com as paredes
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0 && !_isCooldownDamage)
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            _pc = collision.gameObject.GetComponent<PlayerController>();
            if (_pp == null) return;

            _pc.SetAttackEnemyPosition(transform.position);
            _pp.TakeDamage(_damage);
            _isCooldownDamage = true;
        }
    }

    private void Update()
    {
        HasDead();

        if (!_isCooldownDamage) return;
        
        if (_isCooldownDamage && _cooldownTimer < _colldownDamage) _cooldownTimer += Time.deltaTime;
        else
        {
            _isCooldownDamage = false;
            _cooldownTimer = 0f;
        }
    }

    public void ReceiveDamage(float damage)
    {
        _life -= damage;
    }

    public float GetCurrentLife() => _life;

    private void HasDead()
    {
        if (_life <= 0 && !_isDead)
        {
            if (onTucanoRexDead != null) onTucanoRexDead(gameObject.transform.parent.gameObject);
            _isDead = true;
        }
    }

    public void RestartTucanoRex()
    {
        _life = _maxLife;
        _isDead = false;
        _anim.Rebind();
        _anim.Update(0f);
    }
}
