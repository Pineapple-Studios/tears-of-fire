using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal movement")]
    public float MoveSpeed = 10f;
    public Vector2 Direction;
    public bool IsFacingRight = true;

    [Header("Vertical Movement")]
    public float JumpSpeed = 15f;
    public float JumpDelay = 0.25f;
    public float JumpTimer = 0f;
    public float GravityScale = 50f;

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
        Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Jump")) JumpTimer = Time.time + JumpDelay;
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0) _rb.velocity = new Vector2(_rb.velocity.x, 0);

        // Condicionais
        _onGround = 
            Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _distanceToGround, _groundLayer) || 
            Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _distanceToGround, _groundLayer);

        // Aplicando mecânicas
        MoveCharacter();
        
        if (!_playerProps.IsTakingDamage) MoveCharacter();

        // Efeitos dependentes do Player
        CameraFollower();
    }

    private void FixedUpdate()
    {
        _canJump = JumpTimer > Time.time && _onGround;

        if (_canJump) Jump();
    }

    /// <summary>
    /// Define o movimento horizontal do personagem
    /// </summary>
    void MoveCharacter()
    {
        float horizontal = Direction.x;

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
