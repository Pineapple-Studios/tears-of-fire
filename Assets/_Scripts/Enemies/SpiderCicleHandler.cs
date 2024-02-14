using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpiderCicleHandler : MonoBehaviour
{
    [Header("Props")]
    [Tooltip("Tempo entre pulos")]
    [SerializeField]
    private float _jumpColldown = 1f;
    //[Tooltip("Tempo sem identificar o player para parar de se mover")]
    //[SerializeField]
    //private float _movimentCooldown = 5f;

    private SpiderPathVerifier _spv;
    private bool _canJump = true;

    private float _jumpTimer = 0f;

    private void Start()
    {
        _spv = gameObject.GetComponent<SpiderPathVerifier>();
    }

    private void Update()
    {
        if (_spv.IsJumping()) _canJump = false;

        if (!_canJump && _jumpTimer < _jumpColldown) _jumpTimer += Time.deltaTime;
        if (!_canJump && _jumpTimer >= _jumpColldown)
        {
            _canJump = true;
            _jumpTimer = 0f;
        }
    }

    public bool CanJump() => _canJump;
}
