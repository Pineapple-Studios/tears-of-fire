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
        _life -= damage;
        BackImpulse();
        onChangePlayerLife(_life);

        StartCoroutine(EndOfEffects());
    }

    /// <summary>
    /// Empurra o personagem no sentido contrário ao da caminhada
    /// </summary>
    private void BackImpulse()
    {
        
        if (_tr.rotation.y >= 0)
            _rb.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        else 
            _rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
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
