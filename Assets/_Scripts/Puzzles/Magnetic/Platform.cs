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
    private Vector3 _initialPos = Vector3.zero;
    private float _exitTimer = 0f;
    private bool _startCounting = false;
    private bool _endMoviment = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_pc == null)
        {
            if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
            {
                _pc = collision.gameObject.GetComponent<PlayerController>();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _startCounting = true;
        if (_pc != null) _pc.IncreaseExternalVelocity(Vector2.zero);
    }


    void Start()
    {
        _initialPos = transform.position;
    }

    private void Update()
    {
        if (_pc != null) UpdateVelocity();
        if (!_startCounting) return;

        if (_exitTimer < _delayToIdentifyExit && _pc != null)
        {
            _exitTimer += Time.deltaTime;
        }
        else
        {
            _pc = null;
        }
        

        if (_pc == null)
        {
            _exitTimer = 0f;
            _startCounting = false;
        }
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
        _pc = null;
    }

    public void StartPlatformMoviment()
    {
        
    }
}
