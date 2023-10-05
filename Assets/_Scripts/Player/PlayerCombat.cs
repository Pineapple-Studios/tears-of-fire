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

    public static PlayerCombat instance;
    public bool IsAttacking = false;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayers;

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
            Debug.Log("Attack to top/down");
            AttackPoint.localPosition = new Vector3(0, mag.y * _distanceToPlayer, 0);   
        }
        else 
        {
            Debug.Log("Attack to player direction");
            AttackPoint.localPosition = new Vector3(_distanceToPlayer, AttackRange / 3, 0);
        }
    }

    private void Attack()
    {
        TurnToRightAttackDirection();

        // Play an attack animation
        IsAttacking = true;

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
        
        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit {enemy.name} with damage {_pp.GetCurrentDamage()}");
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null) e.TakeDamage(_pp.GetCurrentDamage());
        }
    }
}
