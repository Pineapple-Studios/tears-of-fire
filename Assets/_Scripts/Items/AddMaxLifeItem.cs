using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaxLifeItem : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    [Header("Props")]
    [SerializeField]
    private int _maxLifeToAdd = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            collision.gameObject.GetComponentInChildren<PlayerProps>().AddMaxLife(_maxLifeToAdd);
            
            Destroy(gameObject);
        }
    }
}
