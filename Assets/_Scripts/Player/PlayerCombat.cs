using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerProps))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public Transform AttackPoint;

    [Header("Props")]
    [SerializeField]
    private float _distanceToPlayer = 2f;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _breakableBlockLayer;

    // Isso está exposto
    public static PlayerCombat instance;
    public bool IsAttacking = false;
    public float AttackRange = 0.5f;
    

    private PlayerProps _pp;
    private Vector2 _attackDirection;

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _pp = GetComponent<PlayerProps>();
    }

    void Update()
    {
        _attackDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.R)) Attack();
        if (Input.GetKeyUp(KeyCode.R)) IsAttacking = false;
    }

    private void TurnToRightAttackDirection()
    {
        Vector2 mag = _attackDirection.normalized;
        float currentX = mag.x > 0 ? mag.x : mag.x * -1;
        float currentY = mag.y > 0 ? mag.y : mag.y * -1;

        if (currentY > currentX)
        {
            // Debug.Log("Attack to top/down");
            AttackPoint.localPosition = new Vector3(0, mag.y * _distanceToPlayer, 0);   
        }
        else 
        {
            // Debug.Log("Attack to player direction");
            AttackPoint.localPosition = new Vector3(_distanceToPlayer, AttackRange / 3, 0);
        }
    }

    private void Attack()
    {
        TurnToRightAttackDirection();

        // Play an attack animation
        IsAttacking = true;

        EnemyHit();

        HitBlock();
        HitPuzzleElement();
    }

    private void HitPuzzleElement()
    {
        // Collider2D[] hitBlocks = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange);
    }

    /// <summary>
    /// Verifica se a área de dano tem algum inimigo e executa o dano no inimigo
    /// </summary>
    private void EnemyHit()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, _enemyLayer);

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Transform tParent = enemy.gameObject.transform.parent;
            // Debug.Log($"Hit {tParent.gameObject.name} with damage {_pp.GetCurrentDamage()}");
            Enemy e = tParent.gameObject.GetComponentInChildren<Enemy>();
            if (e != null) e.TakeDamage(_pp.GetCurrentDamage());
        }
    }

    /// <summary>
    /// Verifica se a área de dano tem alguma parede quebrável e executa um hit
    /// </summary>
    private void HitBlock()
    {
        Collider2D[] hitBlocks = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, _breakableBlockLayer);

        foreach (Collider2D block in hitBlocks)
        {
            BreakableBlock b = block.gameObject.transform.parent.gameObject.GetComponentInChildren<BreakableBlock>();
            if (b != null) b.HitWall();
        }
    }
}
