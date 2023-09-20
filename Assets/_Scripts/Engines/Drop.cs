using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private LayerMask _leakLayer;
    [SerializeField]
    private LayerMask _targetLayer;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = new Vector3(
            gameObject.transform.position.x, 
            gameObject.transform.position.y, 
            gameObject.transform.position.z
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _leakLayer) != 0))
        {
            Restart();
        }

        if ((((1 << collision.gameObject.layer) & _targetLayer) != 0))
        {
            Debug.Log(collision.gameObject.name);
            PlayerProps _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp != null) _pp.TakeDamage(20f); // Dano da gota

            Restart();
        }
    }

    private void Restart()
    {
        gameObject.SetActive(false);
        transform.position = _initialPosition;
    }
}
