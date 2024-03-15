using Cinemachine;
using System.Collections;
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
    [SerializeField]
    private Vector3 _offset = Vector3.zero;
    [SerializeField]
    private float _knockbackHitForce = 40f;

    [Header("Refining Props")]
    [SerializeField]
    private float _indulgenceTimeBossHit = 0.5f;

    // Isso est� exposto
    public static PlayerCombat instance;
    public bool IsAttacking = false;
    public float AttackRange = 0.5f;

    private PlayerProps _pp;
    private PlayerController _pc;
    private PlayerInputHandler _pih;
    private Rigidbody2D _rb;
    private Vector2 _attackDirection;

    private bool _shouldBossReceiveAttack = false;
    private float _indulgenceTimer = 0f;


    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
    }

    public void Awake()
    {
        instance = this;
        _pih = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        _pp = GetComponent<PlayerProps>();
        _pc = gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        _attackDirection = _pih.GetDirection();

        if (_shouldBossReceiveAttack && Time.timeScale > 0)
        {
            if (_indulgenceTimer <= _indulgenceTimeBossHit)
            {
                TucanoRexHit();
                EnemyHit();
                _indulgenceTimer += Time.deltaTime;
            }
            else
            {
                _indulgenceTimer = 0;
                _shouldBossReceiveAttack = false;
            }
        }
    }

    private void OnEnable()
    {
        if (_pih != null)
        {
            _pih.KeyAttackDown += Attack; 
            _pih.KeyAttackUp += ReleaseAttack;
        }
    }

    private void OnDisable()
    {
        if (_pih != null)
        {
            _pih.KeyAttackDown -= Attack;
            _pih.KeyAttackUp -= ReleaseAttack;
        }
    }

    private void TurnToRightAttackDirection()
    {
        Vector2 mag = _attackDirection.normalized;
        float currentX = mag.x > 0 ? mag.x : mag.x * -1;
        float currentY = mag.y > 0 ? mag.y : mag.y * -1;

        if (currentY >= currentX)
        {   
            AttackPoint.localPosition = new Vector3(0, mag.y * _distanceToPlayer, 0);   
        }
        else 
        {
            AttackPoint.localPosition = new Vector3(_distanceToPlayer, AttackRange / 3, 0);
        }
    }

    private void Attack()
    {
        TurnToRightAttackDirection();

        // Play an attack animation
        IsAttacking = true;

       // EnemyHit();

        HitBlockByRaycast();

        _shouldBossReceiveAttack = true;
    }

    public void ReleaseAttack()
    {
        // Play an attack animation
        IsAttacking = false;
    }

    /// <summary>
    /// Verifica se a �rea de dano tem algum inimigo e executa o dano no inimigo
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
                if (_attackDirection.y < 0 && !_pc.IsOnGound()) KnockbackToDirection(Vector2.up);
            }
        }
    }

    /// <summary>
    /// Verifica se a �rea de dano tem alguma parede quebr�vel e executa um hit
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
    /// Verifica se a �rea de dano tem algum TucanoRex
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
                if (_attackDirection.y < 0 && !_pc.IsOnGound()) KnockbackToDirection(Vector2.up);
            }
        }
    }

    private void KnockbackToDirection(Vector2 direction)
    {
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = _pc.GetGravityToKnockback();
        _rb.AddForce(direction.normalized * _knockbackHitForce, ForceMode2D.Impulse);
    }
}
