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

    public static PlayerCombat instance;
    public bool IsAttacking = false;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayers;

    private PlayerProps _pp;

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
        if (Input.GetKeyDown(KeyCode.R)) Attack();       
    }

    private void Attack()
    {
        // Play an attack animation
        IsAttacking = true;
        
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
        
        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit {enemy.name} with damage {_pp.GetCurrentDamage()}");
            enemy.GetComponent<Enemy>().TakeDamage(_pp.GetCurrentDamage());
        }
    }
}
