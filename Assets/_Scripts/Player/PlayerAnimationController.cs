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
    private const string RUN = "isRunning";
    private const string ATTACK = "isAttacking";

    // Variáveis de controle
    private bool _isRunning;

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
}
