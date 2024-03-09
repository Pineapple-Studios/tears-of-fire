using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask _magneticHookMask;

    private Transform _attackPoint;
    private float _attackRange;

    private PlayerCombat _pc;
    private PlayerInputHandler _pih;

    void Awake()
    {
        _pih = GetComponent<PlayerInputHandler>();
    }

    void Start()
    {
        _pc = GetComponent<PlayerCombat>();
        _attackPoint = _pc.AttackPoint;
        _attackRange = _pc.AttackRange;
    }

    private void OnEnable()
    {
        if (_pih != null)
        {
            _pih.KeyAttackUp += CheckAllPuzzles;
        }
    }

    private void OnDisable()
    {
        if (_pih != null)
        {
            _pih.KeyAttackUp -= CheckAllPuzzles;
        }
    }

    private void CheckAllPuzzles()
    {
        CheckPuzzleElements();
        CheckMagneticHitElement();
    }

    private void CheckPuzzleElements()
    {
        Collider2D[] hitBlocks = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);

        // Executar puzzle
        foreach (Collider2D block in hitBlocks)
        {
            MagneticPuzzle mp = block.gameObject.GetComponentInParent<MagneticPuzzle>();
            if (mp != null) mp.GoToNextPoint();
        }
    }

    /// <summary>
    /// Right one
    /// </summary>
    private void CheckMagneticHitElement()
    {
        Collider2D[] hitBlocks = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);

        // Executar puzzle
        foreach (Collider2D block in hitBlocks)
        {
            MagneticHitElement mp = block.gameObject.GetComponent<MagneticHitElement>();
            if (mp != null) mp.OnNext();
        }
    }
}
