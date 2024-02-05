using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Action<Vector3> NewCheckpoint;

    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private Animator _anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            NewCheckpoint(_spawnPoint.position);
            _anim.SetBool("IsOn", true);
        }
    }
}
