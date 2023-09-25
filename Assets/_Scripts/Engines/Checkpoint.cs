using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Action<Vector3> NewCheckpoint;

    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private Transform _spawnPoint;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            NewCheckpoint(_spawnPoint.position);
        }
    }
}
