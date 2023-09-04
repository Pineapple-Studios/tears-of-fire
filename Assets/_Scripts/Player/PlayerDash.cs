using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Física")]
    [SerializeField]
    private Rigidbody2D _rb;

    [Header("Propriedades da mecânica")]
    [SerializeField]
    private float _dashDuration = 1f;
    [SerializeField]
    private float _dashRecall = 1f;
    [SerializeField]
    private float _dashMultiplyer = 2f;

    public bool IsDashed = false;
    private bool canDash = true;

    private void Start()
    {
        canDash = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && canDash)
        {
            Debug.Log("Dash");
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        IsDashed = true;
        Vector2 prevVelocity = _rb.velocity;
        _rb.velocity = new Vector2(_rb.velocity.x * _dashMultiplyer, 0);
        yield return new WaitForSeconds(_dashDuration);
        _rb.velocity = new Vector2(prevVelocity.x, _rb.velocity.y);
        IsDashed = false;
        yield return new WaitForSeconds(_dashRecall);
        canDash = true;
    }
}
