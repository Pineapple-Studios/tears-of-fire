using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InstaKillElement : MonoBehaviour
{
    private PlayerProps _pp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) // Layer 3 é a layer do player
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            InstantKill();
        }
    }

    private void InstantKill()
    {
        _pp.TakeDamage(_pp.GetLife() + 1); // +1 para garantir que o dano é maior que a vida
    }
}
