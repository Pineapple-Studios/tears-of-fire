using UnityEngine;

public class DropRockScript : MonoBehaviour
{
    [Header("Props")]
    [SerializeField]
    private float _dropSpeed = 20f;
    [SerializeField]
    private float _damage = 20f;

    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private LayerMask _groundLayer;

    private PlayerProps _pp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        // Player
        if (((1 << other.layer) & _playerLayer) != 0)
        {
            _pp = other.GetComponentInChildren<PlayerProps>();
            if (_pp != null)
            {
                _pp.TakeDamageWhithoutKnockback(_damage);
            };

            gameObject.SetActive(false);
        }

        // Ground
        if (((1 << other.layer) & _groundLayer) != 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down, Time.deltaTime * _dropSpeed);
    }
}
