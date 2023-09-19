using System;
using System.Collections;
using UnityEngine;

public class PlayerProps : MonoBehaviour
{
    public static Action<float> onChangePlayerLife;
    public static Action onPlayerDead;

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

    private Rigidbody2D _rb;
    private Transform _tr;

    [Tooltip("Indicador se o personagem est� ou n�o levando dano")]
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
    /// M�todo respons�vel por dar dano no personagem
    /// </summary>
    /// <param name="damage">Poder/For�a do dano</param>
    public void TakeDamage(float damage)
    {
        IsTakingDamage = true;
        _life -= damage;
        BackImpulse(); // Deve ser melhorado
        onChangePlayerLife(_life);

        StartCoroutine(EndOfEffects());
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
        Debug.Log($"Heal to {_life}");
        onChangePlayerLife(_life);
    }

    /// <summary>
    /// Empurra o personagem no sentido contr�rio ao da caminhada
    /// </summary>
    private void BackImpulse()
    {
        
        if (_tr.rotation.y >= 0)
            _rb.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        else 
            _rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
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
        if (_life <= 0)
        {
            Destroy(gameObject);
            onPlayerDead();
        }
    }

    public float GetLife()
    {
        return _life;
    }

    public float GetCurrentDamage()
    {
        return _hitDamage;
    }
}
