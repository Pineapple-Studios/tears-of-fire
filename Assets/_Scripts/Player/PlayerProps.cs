using System;
using System.Collections;
using UnityEngine;

public class PlayerProps : MonoBehaviour
{
    public static Action<float> onChangePlayerLife;
    public static Action<GameObject> onPlayerDead;
    public static Action onPlayerDamaged;

    [SerializeField]
    [Tooltip("Vida do personagem")]
    private float _life = 100f;
    [SerializeField]
    [Tooltip("Vida máxima do personagem")]
    private float _maxLife = 100f;
    [SerializeField]
    [Tooltip("Dano do personagem")]
    private float _hitDamage = 35f;
    [SerializeField]
    [Tooltip("Tempo para voltar a controlar o personagem depois que ele leva um dano")]
    private float _recallTime = 0.2f;
    [SerializeField]
    [Tooltip("Tempo com o colisor desativado após o dano")]
    public float RecallColliderTime = 0.5f;

    private Rigidbody2D _rb;
    private Transform _tr;

    [Tooltip("Indicador se o personagem está ou não levando dano")]
    public bool IsTakingDamage = false;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _tr = GetComponentInParent<Transform>();
    }

    private void Update()
    {
        HasDied();
    }
    
    /// <summary>
    /// Método responsável por dar dano no personagem
    /// </summary>
    /// <param name="damage">Poder/Força do dano</param>
    public void TakeDamage(float damage)
    {
        IsTakingDamage = true;
        if (FeedbackAndDebugManager.Instance != null && !FeedbackAndDebugManager.Instance.IsInfinityLifeActive()) _life -= damage;
        if (onChangePlayerLife != null) onChangePlayerLife(_life);
        if (onChangePlayerLife != null) onPlayerDamaged();


        StartCoroutine(EndOfEffects());
    }

    /// <summary>
    /// Método responsável por dar dano no personagem sem executar o knockback
    /// </summary>
    /// <param name="damage">Poder/Força do dano</param>
    public void TakeDamageWhithoutKnockback(float damage)
    {
        IsTakingDamage = false;
        if (FeedbackAndDebugManager.Instance != null && !FeedbackAndDebugManager.Instance.IsInfinityLifeActive()) _life -= damage;
        if (onChangePlayerLife != null) onChangePlayerLife(_life);
        if (onChangePlayerLife != null) onPlayerDamaged();

        StartCoroutine(EndOfEffects());
    }

    /// <summary>
    /// Método responsável por recuperar vida do personagem
    /// </summary>
    /// <param name="amount">Heal de vida</param>
    public void HealLife(float amount)
    {
        // Não executa a ação de recuperar vida caso a vida estiver cheia
        if (_life == _maxLife) return;

        _life += amount;
        Debug.Log($"Heal to {_life}");
        onChangePlayerLife(_life);
    }

    /// <summary>
    /// Retorna as funções de movimento do personagem
    /// </summary>
    private IEnumerator EndOfEffects()
    {
        yield return new WaitForSeconds(_recallTime);
        IsTakingDamage = false;
    }

    /// <summary>
    /// Verifica se o personagem ainda está vivo, caso contrário o destroi
    /// </summary>
    private void HasDied()
    {
        if (_life <= 0)
        {
            IsTakingDamage = false;
            onPlayerDead(gameObject.transform.parent.gameObject);
        }
    }

    public float GetLife()
    {
        return _life;
    }

    public void FullHeal()
    {
        _life = _maxLife;
        onChangePlayerLife(_life);
    }

    public float GetCurrentDamage()
    {
        return _hitDamage;
    }

    public bool IsFullLife()
    {
        return _life == _maxLife;
    }

    public void AddMaxLife(int _addMaxContainer)
    {
        float tmp = _maxLife / 20;
        tmp += _addMaxContainer;
        _maxLife = (int)tmp * 20; // 20 é o divisor da UI
        FullHeal();
    }
}
