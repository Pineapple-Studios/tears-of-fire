using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _animator;


    // Identificadores das animações
    private const string GROUND = "onGround";
    private const string RUN = "isRunning";
    private const string ATTACK = "isAttacking";
    private const string JUMP = "isJump";
    private const string FALL = "isFalling";
    private const string DASH = "isDashing";

    // Variáveis de controle
    private bool _isRunning;

    private void OnEnable()
    {
        PlayerProps.onPlayerDamaged += StartDamage;
        PlayerController.onPlayerJumping += StartJump;
        PlayerController.onPlayerFalling += StartFall;
        PlayerController.onPlayerGround += ClearAllStates;
        PlayerDash.onPlayerDashing += StartDash;
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDamaged -= StartDamage;
        PlayerController.onPlayerJumping -= StartJump;
        PlayerController.onPlayerFalling -= StartFall;
        PlayerController.onPlayerGround -= ClearAllStates;
        PlayerDash.onPlayerDashing -= StartDash;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerAttackAnimation();
            return;
        }

        if (_rb.velocity.x != 0 && !_isRunning) _isRunning = true;
        if (_rb.velocity.x == 0 && _isRunning) _isRunning = false;

        SetIsRunningAnimation();
    }

    private void SetIsRunningAnimation()
    {
        if (_isRunning == _animator.GetBool(RUN)) return;
        _animator.SetBool(RUN, _isRunning);
    }

    private void TriggerAttackAnimation()
    {
        _animator.SetBool(ATTACK, true);
    }

    private void ClearAllStates()
    {
        _animator.SetBool(GROUND, false);
        _animator.SetBool(RUN, false);
        _animator.SetBool(ATTACK, false);
        _animator.SetBool(JUMP, false);
        _animator.SetBool(FALL, false);
        _animator.SetBool(DASH, false);
    }

    private void StartRun()
    {
        ClearAllStates();
        _animator.SetBool(RUN, true);
    }

    private void StartJump()
    {
        ClearAllStates();
        _animator.SetBool(JUMP, true);
    }

    private void StartFall()
    {
        ClearAllStates();
        _animator.SetBool(FALL, true);
    }

    private void StartDash()
    {
        ClearAllStates();
        _animator.SetBool(DASH, true);
    }

    private void StartDamage()
    {
        ClearAllStates();
        _animator.SetBool("isDamaged", true);
    }
}
