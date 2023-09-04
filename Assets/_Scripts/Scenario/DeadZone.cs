using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public static Action PlayerOnDeadZone;

    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private Transform _checkpoint;
    // [SerializeField]
    // private CameraFollow _cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Destroy(collision.gameObject);
            GameObject _go = Instantiate(_playerPrefab, _checkpoint);
            // _cam.Target = _go.transform;
        }
    }

    private void Start()
    {
       //  GameObject _go = Instantiate(_playerPrefab, _checkpoint);
        // _cam.Target = _go.transform;
    }
}
