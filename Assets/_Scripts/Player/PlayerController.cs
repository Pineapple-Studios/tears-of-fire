using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public static Action onPlayerJumping;
    public static Action onPlayerFalling;
    public static Action onPlayerGround;
    public static Action onPlayerRunning;
    public static Action onPlayerFreeze;

    [Header("Horizontal movement")]
    public float MoveSpeed = 10f;
    public Vector2 Direction;
    public bool IsFacingRight = true;
    [SerializeField]
    private float _knockBackForce = 50f;

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
    private Vector3 _roofColliderOffset;
    [SerializeField]
    private float _distanceToGround = 0.6f;
    [SerializeField]
    private Vector3 _colliderOffset;
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Camera elements")]
    [SerializeField] 
    private GameObject _cameraFollowGameObject;

    // Components
    private Rigidbody2D _rb;

    // Controladores das mec�nicas
    private PlayerDash _playerDash;
    private PlayerInputHandler _playerInputHandler;

    // Vari�veis de tempor�rias
    private bool _canJump = true;
    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;
    private bool isFalling = false;
    private float _coyoteCounter = 0f;
    private bool _isRespawning = false;
    private bool _isInputDisabled = false;

    private Vector3 _knockPos;
    private Vector3 _enemyAttackPosition;
    private Vector2 _externalVelocity = Vector2.zero;

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
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            transform.position + _roofColliderOffset,
            transform.position + _roofColliderOffset + Vector3.up * _distanceToGround
        );
    }

    private void Awake()
    {
        _playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        PlayerProps.onPlayerDead += Respawn;
 
        if (_playerInputHandler != null)
        {
            _playerInputHandler.KeyJumpDown += OnKeyJumpDown;
            _playerInputHandler.KeyJumpUp += OnKeyJumpUp;
        }
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDead -= Respawn;
        
        if (_playerInputHandler != null)
        {
            _playerInputHandler.KeyJumpDown -= OnKeyJumpDown;
            _playerInputHandler.KeyJumpUp -= OnKeyJumpUp;
        }
    }

    private void OnKeyJumpUp()
    {
        if (ShouldDisbleInput()) return;

        if (_rb.velocity.y > 0) _rb.velocity = new Vector2(_rb.velocity.x, 0);
    }

    private void OnKeyJumpDown()
    {
        if (ShouldDisbleInput()) return;

        JumpTimer = Time.time + CoyoteTime;
    }


    private void Start()
    {
        // Fisica
        _rb = GetComponent<Rigidbody2D>();

        // Mec�nicas
        _playerDash = GetComponentInChildren<PlayerDash>();

        // Camera
        if (_cameraFollowGameObject != null)
        {
            _cameraFollowObject = _cameraFollowGameObject.GetComponent<CameraFollowObject>();
            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        }
    }


    private void Update()
    {
        if (ShouldDisbleInput()) return;

        // Condicionais
        _onGround = Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _distanceToGround, _groundLayer) ||
            Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _distanceToGround, _groundLayer);

        // Movendo personagem
        MoveCharacter();

        // Efeitos dependentes do Player
        CameraFollower();

        // if (_onRoof) _rb.velocity = new Vector2(_rb.velocity.x, 0);

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

        if (_rb.velocity.y > 0 && !_onGround)
        {
            onPlayerJumping();
        }

        // Ajustando velocidade da queda
        if (_rb.velocity.y < 0 && !_onGround)
        {
            onPlayerFalling();
            _rb.gravityScale = GravityScale + _fallVelocityMultiplayer;
            // Definindo velocidade m�xima da queda, a multiplica��o por -1 � porq a velocidade de queda �
            // negativa
            if (_rb.velocity.y < (_maxFallVelocity * -1)) 
                _rb.velocity = new Vector2(_rb.velocity.x, _maxFallVelocity * -1);

            isFalling = true;
        }

        // Se o personagem cair no ch�o 
        if (isFalling && _onGround)
        {
            onPlayerGround();
            isFalling = false;
            _rb.velocity = Vector2.zero;
            // Zerando força externa ao cair no chão
            _externalVelocity = Vector2.zero;
        }
    }

    public bool ShouldDisbleInput()
    {
        return _playerDash.IsDashed || _isRespawning || _isInputDisabled;
    }

    private void Respawn(GameObject obj)
    {
        _isRespawning = true;
        _isInputDisabled = false;
        _externalVelocity = Vector2.zero;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = GravityScale;
    }

    public void Respawn()
    {
        _isRespawning = true;
        _isInputDisabled = false;
        _externalVelocity = Vector2.zero;
        _rb.velocity = Vector2.zero;
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
        float horizontal = _playerInputHandler.GetDirection().x;
        if (horizontal != 0 && _onGround) onPlayerRunning();
        if (horizontal == 0 && _onGround) onPlayerGround();

        _rb.velocity = new Vector2(horizontal * MoveSpeed, _rb.velocity.y) + _externalVelocity;
        
        // Inverte o sprite do personagem caso o jogador tenha invertido o movimento
        if ((horizontal > 0 && !IsFacingRight) || (horizontal < 0 && IsFacingRight))
        {
            Flip();
        }
    }

    /// <summary>
    /// Empurra o personagem no sentido contr�rio ao da caminhada
    /// </summary>
    public void BackImpulse()
    {
        _knockPos = transform.position;

        Vector3 direction = transform.position - _enemyAttackPosition;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = GetGravityToKnockback();
        _rb.AddForce(direction.normalized * _knockBackForce, ForceMode2D.Impulse);
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
        _rb.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        _rb.gravityScale = GravityScale;
        if (onPlayerJumping != null) onPlayerJumping();

        JumpTimer = 0;

        FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.jumpSvart, this.transform.position);
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

    public void EnableInput()
    {
        _isInputDisabled = false;
    }

    public void IncreaseExternalVelocity(Vector2 vel)
    {
        _externalVelocity = vel;
    }

    public bool IsOnGound() => _onGround;

    public float GetGravityToKnockback() => GravityScale + _fallVelocityMultiplayer;

    public void FreezeMovement() {

        _externalVelocity = Vector2.zero;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = GravityScale;
        if (onPlayerFreeze != null) onPlayerFreeze();
    }
}
