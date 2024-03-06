using UnityEngine;

public class RocksCollision : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private float Damage = 20;

    private PlayerProps _pp;

    private void OnParticleCollision(GameObject other) 
    {
        if (((1 << other.layer) & _playerLayer) != 0)
        {
            _pp = other.GetComponentInChildren<PlayerProps>();
            if (_pp == null) return;

            _pp.TakeDamageWhithoutKnockback(Damage);
        }
    }

    
}
