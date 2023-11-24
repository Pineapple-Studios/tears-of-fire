using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public static Action onPlayerJumping;
    public static Action onPlayerFalling;
    public static Action onPlayerGround;
    public static Action onPlayerRunning;

    [Header("Horizontal movement")]
    public float MoveSpeed = 10f;
    public Vector2 Direction;
    public bool IsFacingRight = true;
    [SerializeField]
    private float _knockBackForce = 50f;
    [SerializeField]
    private float _knockDistance = 1f;

    [Header("Vertical Movement")]
    public float JumpSpeed = 15f;
    public float CoyoteTime = 0.25f;
    public float JumpTimer = 0f;
    public float GravityScale = 50f;
    [SerializeField]
    private float _maxFallVelocity = 30f;

    [Header("Jump props")]
    [SerializeField]
    private float _horizontalVelocityMultiplayerOnAir = 0.6f;
    [SerializeField]
    private float _fallVelocityMultiplayer = 1.2f;

    [Header("Collision")]
    [SerializeField]
    private bool _onGround = false;
    [SerializeField]
    private float _distanceToGround = 0.6f;
    [SerializeField]
    private Vector3 _colliderOffset;
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Camera elements")]
    [SerializeField] 
    private GameObject _cameraFollowGameObject;
    
    [Header("Actions")]
    [SerializeField]
    private InputActionAsset Actions;

    // Components
    private Rigidbody2D _rb;

    // Controladores das mec�nicas
    private PlayerProps _playerProps;
    private PlayerDash _playerDash;

    // Vari�veis de tempor�rias
    private bool _canJump = true;
    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;
    private bool isFalling = false;
    private float _coyoteCounter = 0f;
    private bool _isKnocked = false;
    private bool _isRespawning = false;
    private bool _isInputDisabled = false;
    private bool _isJumpPressed;
    private bool _isJumpReleased;

    private Vector3 _knockPos;
    private Vector3 _enemyAttackPosition;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position + _colliderOffset,
            transform.position + _colliderOffset + Vector3.down * _distanceToGround
        );
        Gizmos.DrawLine(
            transform.position - _colliderOffset,
            transform.position - _colliderOffset + Vector3.down * _distanceToGround
        );
    }

    private void Awake()
    {
        Actions.FindActionMap("Gameplay").FindAction("Movement").performed += OnMove;
        Actions.FindActionMap("Gameplay").FindAction("Movement").canceled += OnMove;
        Actions.FindActionMap("Gameplay").FindAction("Jump").performed += OnJump;
        Actions.FindActionMap("Gameplay").FindAction("Jump").canceled += OnJumpReleased;
    }

    private void OnEnable()
    {
        PlayerProps.onPlayerDead += Respawn;
        Actions.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDead -= Respawn;
        Actions.FindActionMap("Gameplay").Disable();
    }

    private void Start()
    {
        // Fisica
        _rb = GetComponent<Rigidbody2D>();

        // Mec�nicas
        _playerDash = GetComponentInChildren<PlayerDash>();
        _playerProps = GetComponentInChildren<PlayerProps>();

        // Camera
        if (_cameraFollowGameObject != null)
        {
            _cameraFollowObject = _cameraFollowGameObject.GetComponent<CameraFollowObject>();
            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        }
    }


    private void Update()
    {
        if (_playerDash.IsDashed) return;
        if (_isRespawning) return;
        if (_isInputDisabled) return;

        // Inputs
        if (_isJumpPressed) JumpTimer = Time.time + CoyoteTime;
        if (_isJumpReleased && _rb.velocity.y > 0) _rb.velocity = new Vector2(_rb.velocity.x, 0);

        // Condicionais
        _onGround = Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _distanceToGround, _groundLayer) ||
            Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _distanceToGround, _groundLayer);

        // Aplicando mec�nicas
        if (_playerProps.IsTakingDamage)
        {
            BackImpulse();
            if (_isKnocked && Vector2.Distance(transform.position, _knockPos) > _knockDistance)
            {
                _rb.velocity = Vector2.zero;
            }
        }
        else
        {
            _isKnocked = false;
            MoveCharacter();
        }

        // Efeitos dependentes do Player
        CameraFollower();

        if (!_onGround)
        {
            // Desacelerando player enquanto est� no ar 
            _rb.velocity = new Vector2(_rb.velocity.x * _horizontalVelocityMultiplayerOnAir, _rb.velocity.y);
            // Aumentando o coyote timer
            _coyoteCounter += Time.deltaTime;
        }
        
        if (_onGround)
        {
            // Alterando velocidade no ar
            _rb.gravityScale = GravityScale;
            // Zerando o coyote timer
            _coyoteCounter = 0f;
        }

        // Ajustando velocidade da queda
        if (_rb.velocity.y < 0 && !_onGround)
        {
            onPlayerFalling();
            _rb.gravityScale = GravityScale + _fallVelocityMultiplayer;
            // Definindo velocidade m�xima da queda, a multiplica��o por -1 � porq a velocidade de queda �
            // negativa
            if (_rb.velocity.y < (_maxFallVelocity * -1)) _rb.velocity = new Vector2(_rb.velocity.x, _maxFallVelocity * -1);
            isFalling = true;
        }

        // Se o personagem cair no ch�o 
        if (isFalling && _onGround)
        {
            onPlayerGround();
            isFalling = false;
            _rb.velocity = Vector2.zero;
        }
    }

    private void Respawn(GameObject obj)
    {
        _isRespawning = true;
        _isInputDisabled = false;
        _rb.gravityScale = GravityScale;
    }

    private void FixedUpdate()
    {
        _canJump = JumpTimer > Time.time && _onGround || (JumpTimer > Time.time && !_onGround && _coyoteCounter < CoyoteTime);

        if (_canJump) Jump();
    }

    /// <summary>
    /// Define o movimento horizontal do personagem
    /// </summary>
    void MoveCharacter()
    {
        float horizontal = Direction.x;
        if (horizontal != 0 && _onGround) onPlayerRunning();
        if (horizontal == 0 && _onGround) onPlayerGround();

        _rb.velocity = new Vector2(horizontal * MoveSpeed * Time.deltaTime, _rb.velocity.y);
        
        // Inverte o sprite do personagem caso o jogador tenha invertido o movimento
        if ((horizontal > 0 && !IsFacingRight) || (horizontal < 0 && IsFacingRight))
        {
            Flip();
        }
    }

    void MoveCharacterAfftedByExternalForce()
    {
        float horizontal = Direction.x;
        if (horizontal != 0 && _onGround) onPlayerRunning();
        if (horizontal == 0 && _onGround) onPlayerGround();

        _rb.velocity += new Vector2(horizontal * MoveSpeed * Time.deltaTime, _rb.velocity.y);

        // Inverte o sprite do personagem caso o jogador tenha invertido o movimento
        if ((horizontal > 0 && !IsFacingRight) || (horizontal < 0 && IsFacingRight))
        {
            Flip();
        }
    }

    /// <summary>
    /// Empurra o personagem no sentido contr�rio ao da caminhada
    /// </summary>
    private void BackImpulse()
    {
        if (_isKnocked) return;

        _isKnocked = true;
        _knockPos = transform.position;

        Vector3 direction = transform.position - _enemyAttackPosition;
        _rb.AddForce(direction * _knockBackForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Inverte o sentido em que o svart est� olhando
    /// </summary>
    void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.rotation = Quaternion.Euler(0, IsFacingRight ? 0 : 180, 0);

        if (_cameraFollowObject != null) _cameraFollowObject.CallTurn();
    }

    /// <summary>
    /// Executa o pulo do personagem
    /// </summary>
    void Jump()
    {
        _rb.AddForce(Vector2.up * JumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        _rb.gravityScale = GravityScale;
        onPlayerJumping();

        JumpTimer = 0;
    }

    /// <summary>
    /// Controla a din�mica da c�mera de acordo com o movimento do personagem
    /// </summary>
    void CameraFollower()
    {
        if (_cameraFollowGameObject == null) return;

        // Falling
        if (
            _rb.velocity.y < _fallSpeedYDampingChangeThreshold && 
            !CameraManager.instance.IsLerpingYDamping && 
            !CameraManager.instance.LerpedFromPlayerFalling
        )
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if (
            _rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && 
            CameraManager.instance.LerpedFromPlayerFalling
        )
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = true;
        _isJumpReleased = false;
    }

    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        _isJumpPressed = false;
        _isJumpReleased = true;

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
    }

    public void SetAttackEnemyPosition(Vector3 pos)
    {
        _enemyAttackPosition = pos;
    }

    public void ToggleRespawning()
    {
        _isRespawning = !_isRespawning;
    }

    public void DisableInput()
    {
        _isInputDisabled = true;
    }

}
