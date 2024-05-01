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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) // Layer 3 é a layer do player
        {
            _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            InstantKill();
        }
    }

    private void InstantKill()
    {
        _pp.TakeDamageWhithoutKnockback(_pp.GetLife());
    }
}
