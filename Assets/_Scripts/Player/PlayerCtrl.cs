using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    public bool IsFacingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;

    [Header("Camera")]
    [SerializeField] private GameObject _cameraFollowGameObject;

    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;
    private PlayerProps _pp;

    private PlayerDash _playerDash;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _pp = GetComponent<PlayerProps>();
        _playerDash = GetComponent<PlayerDash>();

        _cameraFollowObject = _cameraFollowGameObject.GetComponent<CameraFollowObject>();


        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerDash.IsDashed) return;

        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        // animator.SetBool("onGround", onGround);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (direction.x != Vector2.zero.x) 
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);


        // Falling
        if (rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }

    }
    void FixedUpdate()
    {
        if (_playerDash.IsDashed) return;

        if (!_pp.IsTakingDamage) moveCharacter(direction.x);

        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }

        modifyPhysics();
    }

    void moveCharacter(float horizontal)
    {
         rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !IsFacingRight) || (horizontal < 0 && IsFacingRight))
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }
    
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        // rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        rb.velocity = new Vector2(rb.velocity.x, 10);
        jumpTimer = 0;
        // StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
   
    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

        if (direction.x == 0 && onGround) rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.rotation = Quaternion.Euler(0, IsFacingRight ? 0 : 180, 0);

        _cameraFollowObject.CallTurn();
    }

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position + colliderOffset, 
            transform.position + colliderOffset + Vector3.down * groundLength
        );
        Gizmos.DrawLine(
            transform.position - colliderOffset, 
            transform.position - colliderOffset + Vector3.down * groundLength
        );
    }
}