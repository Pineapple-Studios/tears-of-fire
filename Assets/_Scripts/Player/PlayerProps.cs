using System;
using System.Collections;
using UnityEngine;

public class PlayerProps : MonoBehaviour
{
    private const float LIFE_PART = 20f;

    public static Action<float> onChangePlayerLife;
    public static Action<GameObject> onPlayerDead;
    public static Action onPlayerDamaged;

    [SerializeField]
    [Tooltip("Vida do personagem")]
    private float _life = 100f;
    [SerializeField]
    [Tooltip("Vida m�xima do personagem")]
    private float _maxLife = 100f;
    [SerializeField]
    [Tooltip("Dano do personagem")]
    private float _hitDamage = 35f;
    [SerializeField]
    [Tooltip("Tempo para voltar a controlar o personagem depois que ele leva um dano")]
    private float _recallTime = 0.2f;
    [SerializeField]
    [Tooltip("Tempo com o colisor desativado ap�s o dano")]
    public float RecallColliderTime = 0.5f;
    [SerializeField]
    private float _cooldownAfterHit = 1f;

    [Header("Refining")]
    [SerializeField]
    private float _timeScaleDuringHitPause = 0.1f;
    [SerializeField]
    private float _hitPauseDuration = 0.05f;

    private Rigidbody2D _rb;
    private Transform _tr;
    private PlayerAnimationController _pa;
    private PlayerController _pc;
    private bool _isDead = false;
    private float _cooldownTimer = 0f;
    private bool _hasDamaged = false;

    [Tooltip("Indicador se o personagem est� ou n�o levando dano")]
    public bool IsTakingDamage = false;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _tr = GetComponentInParent<Transform>();
        _pa = GetComponent<PlayerAnimationController>();
        _pc = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        HasDied();

        if (_hasDamaged)
        {
            if (_cooldownTimer < _cooldownAfterHit)
            {
                _cooldownTimer += Time.deltaTime;
            }
            else
            {
                _hasDamaged = false;
                _cooldownTimer = 0f;
            }
        }
    }

    private void HandleReduceHP(float damage)
    {
        if (FeedbackAndDebugManager.Instance != null && FeedbackAndDebugManager.Instance.IsInfinityLifeActive()) return;
        _life -= damage;
    }
    
    /// <summary>
    /// M�todo respons�vel por dar dano no personagem
    /// </summary>
    /// <param name="damage">Poder/For�a do dano</param>
    public void TakeDamage(float damage, bool shouldKnock = true)
    {
        if (_hasDamaged) return; 

        IsTakingDamage = true;
        HandleReduceHP(damage);
        if (onChangePlayerLife != null) onChangePlayerLife(_life);
        if (onPlayerDamaged != null) onPlayerDamaged();
        
        // Knockback
        if (shouldKnock) _pc.BackImpulse();
        _hasDamaged = true;

        // Feedback
        if (RumbleManager.instance != null) RumbleManager.instance.RumblePlayerDamage();
        FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.hitSvart, this.transform.position);
        CinemachineShakeManager.Instance.ShakeCamera();

        FreezingFeedback();
        StartCoroutine(EndOfEffects());
    }

    /// <summary>
    /// M�todo respons�vel por dar dano no personagem sem executar o knockback
    /// </summary>
    /// <param name="damage">Poder/For�a do dano</param>
    public void TakeDamageWhithoutKnockback(float damage)
    {
        TakeDamage(damage, false);
    }

    /// <summary>
    /// M�todo respons�vel por recuperar vida do personagem
    /// </summary>
    /// <param name="amount">Heal de vida</param>
    public void HealLife(float amount)
    {
        // N�o executa a a��o de recuperar vida caso a vida estiver cheia
        if (_life == _maxLife) return;

        _life += amount;
        // Debug.Log($"Heal to {_life}");
        onChangePlayerLife(_life);
    }

    /// <summary>
    /// Retorna as fun��es de movimento do personagem
    /// </summary>
    private IEnumerator EndOfEffects()
    {
        yield return new WaitForSeconds(_recallTime);
        IsTakingDamage = false;
    }

    /// <summary>
    /// Verifica se o personagem ainda est� vivo, caso contr�rio o destroi
    /// </summary>
    private void HasDied()
    {
        if (_life <= 0 && !_isDead)
        {
            if (FMODAudioManager.Instance != null) FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.death, this.transform.position);
            IsTakingDamage = false;
            _isDead = true;
            onPlayerDead(gameObject.transform.parent.gameObject);
        }
    }

    private bool _isFreezing = false;
    private void FreezingFeedback()
    {
        if (_isFreezing) return;
        _isFreezing = true;
        StartCoroutine(HitPauseCoroutine());
    }

    private IEnumerator HitPauseCoroutine()
    {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = _timeScaleDuringHitPause;

        yield return new WaitForSecondsRealtime(_hitPauseDuration);

        Time.timeScale = originalTimeScale;
        _isFreezing = false;
    }

    public float GetLife() =>_life;
    public float GetLifeByUI() => _life / LIFE_PART;
    public float GetCurrentDamage() => _hitDamage;
    public bool IsFullLife() => _life == _maxLife;
    public float GetCurrentMaxLife() => _maxLife;
    public float GetCurrentMaxLifeByUI() => _maxLife / LIFE_PART;

    public void FullHeal()
    {
        _life = _maxLife;
        if (onChangePlayerLife != null) onChangePlayerLife(_life);
        _isDead = false;
    }

    public void AddMaxLife(int _addMaxContainer)
    {
        float tmp = _maxLife / LIFE_PART;
        tmp += _addMaxContainer;
        _maxLife = (int)tmp * LIFE_PART;
        FullHeal();
    }

}
