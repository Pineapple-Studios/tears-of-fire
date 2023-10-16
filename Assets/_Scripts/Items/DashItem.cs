using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItem : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            collision.gameObject.GetComponentInChildren<PlayerDash>().enabled = true;
            LevelDataManager.Instance.SetDashState(true);

            Destroy(gameObject);
        }
    }
}
