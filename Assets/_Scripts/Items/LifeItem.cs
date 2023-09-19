using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : MonoBehaviour
{
    [Header("Props")]
    [SerializeField]
    private float _heallingLife = 20f;

    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            Debug.Log(collision.gameObject.name);
            PlayerProps _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp != null) _pp.HealLife(_heallingLife);

            Destroy(gameObject);
        }
    }
}
