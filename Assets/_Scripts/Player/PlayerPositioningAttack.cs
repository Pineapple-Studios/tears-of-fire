using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransformMemo
{
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
}

public class PlayerPositioningAttack : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField]
    private Transform attackA;
    [SerializeField]
    private Transform attackB;
    [SerializeField]
    private Transform attackC;

    [Header("Casting")]
    [SerializeField]
    private Transform _attackPostion;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private PlayerCombat _playerCombat;

    private TransformMemo originA;
    private TransformMemo originB; 
    private TransformMemo originC;
    public Vector2 AttackDirection;

    private PlayerInputHandler _pih;
    private PlayerController _pc;

    void Awake()
    {
        _pih = transform.parent.GetComponentInChildren<PlayerInputHandler>();
        _pc = transform.parent.GetComponentInChildren<PlayerController>();
    }

    private void Start()
    {
        originA = new TransformMemo
        {
            position = attackA.localPosition + new Vector3(1,0,0),
            rotation = attackA.rotation
        };

        originB = new TransformMemo
        {
            position = attackB.localPosition,
            rotation = attackB.rotation
        };
        originC = new TransformMemo
        {
            position = attackC.localPosition,
            rotation = attackC.rotation
        };
    }

    private void Update()
    {
        AttackDirection = _pih.GetDirection();

        if (attackA.gameObject.activeSelf || attackB.gameObject.activeSelf || attackC.gameObject.activeSelf) return;

        if (AttackDirection == Vector2.zero)
        {
            HandleStopPositions();
            return;
        }

        if (!_pc.IsOnGound())
        {
            JumpingHandler();
            return;
        }

        // ---------------- Rotação
        // Cima
        if (AttackDirection.y > 0 && AttackDirection.x == 0)
        {
            if (attackB.rotation.eulerAngles.x == 180) RotateRelative(_player, attackB, -90f);
            else if (attackB.rotation.eulerAngles.z != 90) RotateRelative(_player, attackB, 90f);
        }

        // Baixo
        if (AttackDirection.y < 0 && AttackDirection.x == 0)
        {
            attackA.rotation = Quaternion.Euler(0f, 0f, 180f);
            if (attackB.rotation.eulerAngles.z != 270) RotateRelative(_player, attackB, -90f);
        }

        // Direita ou esquerda
        if (AttackDirection.y == 0 && AttackDirection.x != 0)
        {
            HandleStopPositions();
            return;
        }

        // ---------------------- Posição
        attackA.position = _attackPostion.position + new Vector3(0, -1, 0);
        attackB.position = _attackPostion.position;
        attackC.position = _attackPostion.position;

        if (AttackDirection.y < 0) // Baixo 
        {  
            attackA.position = _attackPostion.position + new Vector3(0, 1.5f, 0);
            attackB.position = _attackPostion.position + new Vector3(-0.5f, 0, 0);
        }
        
        if (AttackDirection.y > 0) // Cima
        {
            attackB.position = _attackPostion.position + new Vector3(1, 1, 0);
            attackA.position = _attackPostion.position;
        }
    }

    private void JumpingHandler()
    {
        if (AttackDirection.y > 0)
        {
            if (attackB.rotation.eulerAngles.x == 180) RotateRelative(_player, attackB, -90f);
            else if (attackB.rotation.eulerAngles.z != 90) RotateRelative(_player, attackB, 90f);

            attackB.position = _attackPostion.position + new Vector3(1, 1, 0);
            attackA.position = _attackPostion.position;
        }

        if (AttackDirection.y < 0)
        {
            attackA.rotation = Quaternion.Euler(0f, 0f, 180f);
            if (attackB.rotation.eulerAngles.z != 270) RotateRelative(_player, attackB, -90f);

            attackA.position = _attackPostion.position + new Vector3(0, 1.5f, 0);
            attackB.position = _attackPostion.position + new Vector3(-0.5f, 0, 0);
        }

        attackC.position = _attackPostion.position;
    }

    private void HandleStopPositions()
    {
        if (originA.position != attackA.position) CopyTransformMemoValues(originA, attackA);

        if (originB.position != attackB.position) CopyTransformMemoValues(originB, attackB);

        if (originC.position != attackC.position) CopyTransformMemoValues(originC, attackC);

        if (_player.rotation.eulerAngles.y == 180)
        {
            if (attackB.rotation.eulerAngles.z != 180)
            {
                RotateRelative(_player, attackB, 180f);
                attackB.position = _attackPostion.position + new Vector3(0, 1, 0);
            }

            if (attackC.rotation.eulerAngles.z != 180)
            {
                RotateRelative(_player, attackC, 180f);
                attackC.localPosition = new Vector3(-1.7f, 0, 0);
            }
        }
    }

    private void RotateRelative(Transform target, Transform element, float angle)
    {
        // Calculate the position of the object in relation to the centerObject
        Vector3 offset = element.position - target.position;

        // Rotate the object around the centerObject in 2D space
        element.RotateAround(target.position, Vector3.forward, angle);

        // Apply the offset to maintain the relative position
        element.position = target.position + offset;
    }

    private void CopyTransformMemoValues(TransformMemo source, Transform target)
    {
        // Copy position
        target.localPosition = source.position;

        // Copy rotation
        target.rotation = source.rotation;
    }

    public void ReleaseAttack()
    {
        _playerCombat.ReleaseAttack();
    }
}
