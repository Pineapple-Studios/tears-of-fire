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
    private const string DAMEGED = "isDamaged";

    // Clips individuais
    private const string HIT = "clip_hit";
    private const string DEATH = "clip_death";
    private const string RESPAWN = "clip_respawn";

    private PlayerInputHandler _pih;

    void Awake()
    {
        _pih = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        PlayerProps.onPlayerDamaged += StartDamage;
        PlayerProps.onPlayerDead += StartDeath;
        PlayerController.onPlayerJumping += StartJump;
        PlayerController.onPlayerFalling += StartFall;
        PlayerController.onPlayerRunning += StartRun;
        PlayerController.onPlayerGround += ClearAllStates;
        PlayerController.onPlayerFreeze += ClearAllStates;
        PlayerDash.onPlayerDashing += StartDash;

        if (_pih != null)
        {
            _pih.KeyAttackDown += TriggerAttackAnimation;
        }
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDamaged -= StartDamage;
        PlayerProps.onPlayerDead -= StartDeath;
        PlayerController.onPlayerJumping -= StartJump;
        PlayerController.onPlayerFalling -= StartFall;
        PlayerController.onPlayerRunning -= StartRun;
        PlayerController.onPlayerGround -= ClearAllStates;
        PlayerController.onPlayerFreeze -= ClearAllStates;
        PlayerDash.onPlayerDashing -= StartDash;

        if (_pih != null)
        {
            _pih.KeyAttackDown -= TriggerAttackAnimation;
        }
    }

    private void Start()
    {
        ClearAllStates();
    }

    private void TriggerAttackAnimation()
    {
        if (_animator.GetBool(DASH) == true) return;

        ClearAllStates();
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
        _animator.SetBool(DAMEGED, false);
    }

    private void StartRun()
    {
        // Evitando chamar o mesmo estado mais de uma vez
        if (_animator.GetBool(ATTACK) == true || _animator.GetBool(RUN) == true) return;

        ClearAllStates();
        _animator.SetBool(RUN, true);
    }

    private void StartJump()
    {
        // Evitando chamar o mesmo estado mais de uma vez
        if (_animator.GetBool(ATTACK) == true || _animator.GetBool(JUMP) == true) return;

        ClearAllStates();
        _animator.SetBool(JUMP, true);
    }

    private void StartFall()
    {
        // Evitando chamar o mesmo estado mais de uma vez
        if (_animator.GetBool(ATTACK) == true || _animator.GetBool(FALL) == true) return;

        ClearAllStates();
        _animator.SetBool(FALL, true);
    }

    private void StartDash()
    {
        // Evitando chamar o mesmo estado mais de uma vez
        if (_animator.GetBool(DASH) == true) return;

        ClearAllStates();
        _animator.SetBool(DASH, true);
    }

    private void StartDamage()
    {
        // Evitando chamar o mesmo estado mais de uma vez
        if (_animator.GetBool(DAMEGED) == true) return;


        ClearAllStates();
        _animator.SetBool(DAMEGED, true);
        _animator.Play(HIT);
    }

    private void StartDeath(GameObject obj)
    {
        ClearAllStates();
        _animator.Play(DEATH);
    }

    public void StartRespawn()
    {
        ClearAllStates();
        _animator.Play(RESPAWN);
    }
}
