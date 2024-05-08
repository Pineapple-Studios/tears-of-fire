using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private float _delayToIdentifyExit = 1f;

    public bool anim_isAnimating = false;
        
    private PlayerController _pc;
    private PlayerPuzzleHandler _pph;

    private Vector3 _initialPos = Vector3.zero;
    private float _exitTimer = 0f;
    private bool _startCounting = false;
    private bool _endMoviment = false;

    void Start()
    {
        _pph = FindAnyObjectByType<PlayerPuzzleHandler>();
        _pc = FindAnyObjectByType<PlayerController>();
        _initialPos = transform.position;
    }

    private void Update()
    {
        if (!_pph.IsInPlatform())
        {
            _pc.IncreaseExternalVelocity(Vector2.zero);
            return;
        }

        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        // Calc velocity based on Position
        Vector2 vel = (transform.position - _initialPos) / Time.deltaTime;
        _initialPos = transform.position;

        if (!anim_isAnimating && vel != Vector2.zero) vel = Vector2.zero;

        // Y force is applied based on collisions between platform and player
        _pc.IncreaseExternalVelocity(new Vector2(vel.x, 0));
    }

    /// <summary>
    /// Método chamado ao acabar o movimento da animação
    /// </summary>
    public void EndPlatformMoviment()
    {
        _pc.IncreaseExternalVelocity(Vector2.zero);
    }

    public void StartPlatformMoviment()
    {
        
    }
}
