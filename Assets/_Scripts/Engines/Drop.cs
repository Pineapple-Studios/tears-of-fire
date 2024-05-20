using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private LayerMask _leakLayer;
    [SerializeField]
    private LayerMask _targetLayer;
    [SerializeField]
    public float DropSpeed;

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
            Pool pool = collision.gameObject.GetComponent<Pool>();
            Restart(pool);
        }

        if ((((1 << collision.gameObject.layer) & _targetLayer) != 0))
        {
            // Debug.Log(collision.gameObject.name);

            PlayerProps _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp != null) _pp.TakeDamageWhithoutKnockback(20f); // Dano da gota

            Restart();
        }
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, 
            transform.localPosition + new Vector3(-1, 0, 0), 
            Time.deltaTime * DropSpeed
        );
    }

    private void Restart()
    {
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.leak, this.transform.position);

        gameObject.SetActive(false);
        transform.position = _initialPosition;
    }

    private void Restart(Pool pool)
    {
        pool.InstantiateSplashParticles();
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.leak, this.transform.position);

        gameObject.SetActive(false);
        transform.position = _initialPosition;
    }
}
