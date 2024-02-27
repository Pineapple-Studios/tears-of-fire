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

    private TransformMemo originA;
    private TransformMemo originB; 
    private TransformMemo originC;
    private Vector2 _attackDirection;

    private void Start()
    {
        originA = new TransformMemo
        {
            position = attackA.localPosition,
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
        _attackDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (attackA.gameObject.activeSelf || attackB.gameObject.activeSelf || attackC.gameObject.activeSelf) return;
        
        if (_attackDirection == Vector2.zero)
        {
            if (originA.position != attackA.position) CopyTransformMemoValues(originA, attackA);

            if (originB.position != attackB.position) CopyTransformMemoValues(originB, attackB);

            if (originC.position != attackC.position) CopyTransformMemoValues(originC, attackC);

            return;
        }


        // Rotação
        // Cima
        if (_attackDirection.y > 0 && attackB.rotation == Quaternion.identity)
        {
            RotateRelative(_player, attackB, 90f);
        }

        if (_player.rotation.y == 180) RotateRelative(_player, attackB, 180f);

        // Baixo
        if (_attackDirection.y < 0)
        {
            attackA.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        // Posição
        if (_attackDirection.y < 0) // Baixo
            attackA.position = _attackPostion.position + new Vector3(0, 1.5f, 0);
        else if (_attackDirection.y > 0)
            attackA.position = _attackPostion.position;
        else
            attackA.position = _attackPostion.position + new Vector3(0, -1, 0);

        if (_attackDirection.y > 0) // cima
            attackB.position = _attackPostion.position + new Vector3(1, 1, 0);
        else
            attackB.position = _attackPostion.position;

        attackC.position = _attackPostion.position;
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
}
