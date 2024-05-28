using System;
using UnityEngine;

public class TucanoRexProps : MonoBehaviour
{
    public static Action<GameObject> onTucanoRexDead;

    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    [Header("Feedbacks")]
    [SerializeField]
    private VFXPuff _puff;

    [Header("Setup")]
    [SerializeField]
    private Animator _gamePlayAnimController;
    [SerializeField]
    private Animator _feedbackAnimController;
    [SerializeField]
    private BossFightScenarioHandler _bfsh;

    [Header("Props")]
    [SerializeField]
    private float _maxLife = 900;
    [SerializeField]
    private float _life = 900;
    [SerializeField]
    private float _damage = 30;
    [SerializeField]
    private float _colldownDamage = 2;
    [SerializeField]
    private float _colldownReceive = 2;
    [SerializeField]
    private Collider2D[] _colliderList;

    private PlayerProps _pp;
    private PlayerController _pc;
    private bool _isCooldownDamage = false;
    private float _cdToTakeDamage = 0f;
    private bool _isReceivingDamage = false;
    private float _cdToReceiveDamage = 0f;
    private bool _isDead = false;

    private void OnEnable()
    {
        LevelDataManager.onRestartElements += RestartTucanoRex;
    }

    private void OnDisable()
    {
        LevelDataManager.onRestartElements -= RestartTucanoRex;
    }

    private void Start()
    {
        _life = _maxLife;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerCollider(collision);
    }

    public void TriggerCollider(Collider2D collision)
    {
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
        CooldownDamageCounter();
        CooldownReceiveCounter();
    }

    private void CooldownReceiveCounter()
    {
        if (!_isReceivingDamage) return;

        if (_isReceivingDamage && _cdToReceiveDamage < _colldownReceive) _cdToReceiveDamage += Time.deltaTime;
        else
        {
            _isReceivingDamage = false;
            _feedbackAnimController.SetBool("isDamaged", false);
            _cdToReceiveDamage = 0f;
        }
    }

    private void CooldownDamageCounter()
    {
        if (!_isCooldownDamage) return;

        if (_isCooldownDamage && _cdToTakeDamage < _colldownDamage) _cdToTakeDamage += Time.deltaTime;
        else
        {
            _isCooldownDamage = false;
            _cdToTakeDamage = 0f;
        }
    }

    public void ReceiveDamage(float damage)
    {
        if (_isReceivingDamage) return;

        _isReceivingDamage = true;
        _feedbackAnimController.SetBool("isDamaged", true);
        _life -= damage;
        if (_puff != null) _puff.PlayPuff();

        if (RumbleManager.instance != null) RumbleManager.instance.RumbleBossDamage();
    }

    public float GetCurrentLife() => _life;

    private void HasDead()
    {
        if (_life <= 0 && !_isDead)
        {
            foreach (Collider2D col in _colliderList) col.enabled = false;
            if (onTucanoRexDead != null) onTucanoRexDead(gameObject.transform.parent.gameObject);
            _gamePlayAnimController.Play("clip_dead");
            _isDead = true;
        }
    }

    public void RestartTucanoRex()
    {
        _life = _maxLife;
        _isDead = false;

        _gamePlayAnimController.Rebind();
        _gamePlayAnimController.Update(0f);
        _gamePlayAnimController.SetBool("isStarted", false);

        _feedbackAnimController.Rebind();
        _feedbackAnimController.Update(0f);
        _bfsh.RestartScenario();
        // _gamePlayAnimController.SetBool("isStarted", true);
    }
}
