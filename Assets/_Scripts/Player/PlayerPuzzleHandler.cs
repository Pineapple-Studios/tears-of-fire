using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask _magneticHookMask;

    [SerializeField]
    private InputActionAsset Actions;

    private Transform _attackPoint;
    private float _attackRange;

    private PlayerCombat _pc;

    void Awake()
    {
        Actions.FindActionMap("Gameplay").FindAction("Attack").performed += CheckAllPuzzles;
    }

    void Start()
    {
        _pc = GetComponent<PlayerCombat>();
        _attackPoint = _pc.AttackPoint;
        _attackRange = _pc.AttackRange;
    }

    private void OnEnable()
    {
        Actions.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        Actions.FindActionMap("Gameplay").Disable();
    }

    private void CheckAllPuzzles(InputAction.CallbackContext context)
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
