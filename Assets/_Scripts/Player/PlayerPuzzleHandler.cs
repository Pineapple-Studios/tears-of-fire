using UnityEngine;

public class PlayerPuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask _magneticHookMask;

    [Header("Collider")]
    [SerializeField]
    private Vector3 _colliderOffset = Vector3.zero;
    [SerializeField]
    private float _distanceToGround = 2f;
    [SerializeField]
    private LayerMask _platformMask;

    private Transform _attackPoint;
    private float _attackRange;
    private bool _isOnPlatform;
    private RaycastHit2D _rd2D;

    private PlayerCombat _pc;
    private PlayerInputHandler _pih;

    void Awake()
    {
        _pih = GetComponent<PlayerInputHandler>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(256, 256, 0);
        Gizmos.DrawLine(
            transform.position + _colliderOffset,
            transform.position + _colliderOffset + Vector3.down * _distanceToGround
        );
    }

    void Start()
    {
        _pc = GetComponent<PlayerCombat>();
        _attackPoint = _pc.AttackPoint;
        _attackRange = _pc.AttackRange;
    }

    private void OnEnable()
    {
        if (_pih != null)
        {
            _pih.KeyAttackDown += CheckAllPuzzles;
        }
    }

    private void OnDisable()
    {
        if (_pih != null)
        {
            _pih.KeyAttackDown -= CheckAllPuzzles;
        }
    }

    private void CheckAllPuzzles()
    {
        CheckMagneticHitElement();
    }

    private void Update()
    {
        _rd2D = Physics2D.Raycast(
            transform.position + _colliderOffset, 
            Vector2.down, 
            _distanceToGround, 
            _platformMask
        );
        _isOnPlatform = _rd2D.collider != null;
    }

    /// <summary>
    /// Right one
    /// </summary>
    private void CheckMagneticHitElement()
    {
        Collider2D[] hitBlocks = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);

        // Executar puzzle
        foreach (Collider2D block in hitBlocks)
        {
            MagneticHitElement mp = block.gameObject.GetComponent<MagneticHitElement>();

            if (mp != null)
            {
                if (!_isOnPlatform)
                {
                    FeedbackManagerHandler.Instance.NegativeFeedback();
                    return;
                }

                mp.OnNext();
            }
        }
    }

    public bool IsInPlatform() => _isOnPlatform;
}
