using System;
using UnityEngine;

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

    // Components
    private Rigidbody2D _rb;

    // Controladores das mecânicas
    private PlayerProps _playerProps;
    private PlayerDash _playerDash;

    // Variáveis de temporárias
    private bool _canJump = true;
    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;
    private bool isFalling = false;
    private float _coyoteCounter = 0f;

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

    private void Start()
    {
        // Fisica
        _rb = GetComponent<Rigidbody2D>();

        // Mecânicas
        _playerDash = GetComponentInChildren<PlayerDash>();
        _playerProps = GetComponentInChildren<PlayerProps>();

        // Camera
        _cameraFollowObject = _cameraFollowGameObject.GetComponent<CameraFollowObject>();
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    private void Update()
    {
        if (_playerDash.IsDashed) return;

        // Inputs
        Direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (Input.GetButtonDown("Jump")) JumpTimer = Time.time + CoyoteTime;
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0) _rb.velocity = new Vector2(_rb.velocity.x, 0);

        // Condicionais
        _onGround = Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _distanceToGround, _groundLayer) ||
            Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _distanceToGround, _groundLayer);

        // Aplicando mecânicas
        MoveCharacter();
        
        if (!_playerProps.IsTakingDamage) MoveCharacter();

        // Efeitos dependentes do Player
        CameraFollower();

        if (!_onGround)
        {
            // Desacelerando player enquanto está no ar 
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
            // Definindo velocidade máxima da queda, a multiplicação por -1 é porq a velocidade de queda é
            // negativa
            if (_rb.velocity.y < (_maxFallVelocity * -1)) _rb.velocity = new Vector2(_rb.velocity.x, _maxFallVelocity * -1);
            isFalling = true;
        }

        // Se o personagem cair no chão 
        if (isFalling && _onGround)
        {
            onPlayerGround();
            isFalling = false;
            _rb.velocity = Vector2.zero;
        }
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

        _rb.velocity = new Vector2(horizontal * MoveSpeed, _rb.velocity.y);
        
        // Inverte o sprite do personagem caso o jogador tenha invertido o movimento
        if ((horizontal > 0 && !IsFacingRight) || (horizontal < 0 && IsFacingRight))
        {
            Flip();
        }
    }

    /// <summary>
    /// Inverte o sentido em que o svart está olhando
    /// </summary>
    void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.rotation = Quaternion.Euler(0, IsFacingRight ? 0 : 180, 0);

        _cameraFollowObject.CallTurn();
    }

    /// <summary>
    /// Executa o pulo do personagem
    /// </summary>
    void Jump()
    {
        _rb.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        _rb.gravityScale = GravityScale;
        onPlayerJumping();

        JumpTimer = 0;
    }

    /// <summary>
    /// Controla a dinâmica da câmera de acordo com o movimento do personagem
    /// </summary>
    void CameraFollower()
    {
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
}
