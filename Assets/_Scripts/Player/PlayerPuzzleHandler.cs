using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuzzleHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask _magneticHookMask;

    private Transform _attackPoint;
    private float _attackRange;

    private PlayerCombat _pc;

    void Start()
    {
        _pc = GetComponent<PlayerCombat>();
        _attackPoint = _pc.AttackPoint;
        _attackRange = _pc.AttackRange;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) CheckPuzzleElements();
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
}
