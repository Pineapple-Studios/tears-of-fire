using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDashEffect : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private GetDashEffect _dashEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Here");
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            _dashEffect.SetPlayerTransform(collision.transform);
        }
    }
}
