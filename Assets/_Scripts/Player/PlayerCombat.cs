using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

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
    [SerializeField]
    private Vector3 _offset = Vector3.zero;

    [Header("Actions")]
    [SerializeField]
    InputActionAsset Actions;

    // Isso está exposto
    public static PlayerCombat instance;
    public bool IsAttacking = false;
    public float AttackRange = 0.5f;
    

    private PlayerProps _pp;
    private Rigidbody2D _rb;
    private Vector2 _attackDirection;

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
    }

    public void Awake()
    {
        instance = this;
        Actions.FindActionMap("Gameplay").FindAction("Attack").performed += Attack;
        Actions.FindActionMap("Gameplay").FindAction("Attack").canceled += ReleaseAttack;
    }

    private void Start()
    {
        _pp = GetComponent<PlayerProps>();
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        _attackDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void OnEnable()
    {
        Actions.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        Actions.FindActionMap("Gameplay").Disable();
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

    private void Attack(InputAction.CallbackContext context)
    {
        TurnToRightAttackDirection();

        // Play an attack animation
        IsAttacking = true;

        EnemyHit();

        HitBlockByRaycast();

        TucanoRexHit();
    }

    private void ReleaseAttack(InputAction.CallbackContext context)
    {
        // Play an attack animation
        IsAttacking = false;
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
            Transform MainParent = enemy.gameObject.transform;
            while (MainParent.parent != null)
            {
                MainParent = MainParent.parent;
            }

            // Debug.Log($"Hit {MainParent.gameObject.name} with damage {_pp.GetCurrentDamage()}");
            Enemy e = MainParent.gameObject.GetComponentInChildren<Enemy>();
            if (e != null)
            {
                e.TakeDamage(_pp.GetCurrentDamage());
                if (_attackDirection.y < 0) _rb.velocity = Vector2.up * 20f;
            }
        }
    }

    /// <summary>
    /// Verifica se a área de dano tem alguma parede quebrável e executa um hit
    /// </summary>
    private void HitBlockByRaycast()
    {
        Vector3 origin = transform.position + _offset;
        Vector2 forward2D = new Vector2(transform.forward.z, transform.forward.y);  

        RaycastHit2D topBlocks = Physics2D.Raycast(origin + new Vector3(0, AttackRange / 2, 0), forward2D, _distanceToPlayer, _breakableBlockLayer);
        if (topBlocks.collider != null)
        {
            BreakableBlock b = topBlocks.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<BreakableBlock>();
            if (b != null) b.HitWall();
            return;
        }

        RaycastHit2D middleBlocks = Physics2D.Raycast(origin, forward2D, _distanceToPlayer, _breakableBlockLayer);
        if (middleBlocks.collider != null)
        {
            BreakableBlock b = topBlocks.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<BreakableBlock>();
            if (b != null) b.HitWall();
            return;
        }

        RaycastHit2D bottomBlocks = Physics2D.Raycast(origin + new Vector3(0, -(AttackRange / 2), 0), forward2D, _distanceToPlayer, _breakableBlockLayer);
        if (bottomBlocks.collider != null)
        {
            BreakableBlock b = topBlocks.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<BreakableBlock>();
            if (b != null) b.HitWall();
            return;
        }
    }

    /// <summary>
    /// Verifica se a área de dano tem algum TucanoRex
    /// </summary>
    private void TucanoRexHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, _enemyLayer);
        
        foreach (Collider2D boss in hitEnemies)
        {
            Transform MainParent = boss.gameObject.transform;
            while (MainParent.parent != null)
            {
                MainParent = MainParent.parent;
            }

            TucanoRexProps trp = MainParent.gameObject.GetComponentInChildren<TucanoRexProps>();
            if (trp != null)
            {
                trp.ReceiveDamage(_pp.GetCurrentDamage());
                if (_attackDirection.y < 0) _rb.velocity = Vector2.up * 20f;
            }
        }
    }
}
