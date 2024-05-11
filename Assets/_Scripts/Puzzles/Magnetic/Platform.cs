using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;

    public bool anim_isAnimating = false;
        
    private PlayerController _pc;

    private Vector3 _initialPos = Vector3.zero;
    private bool isApplingForce = false;

    void Start()
    {
        _pc = FindAnyObjectByType<PlayerController>();
        _initialPos = transform.position;
    }

    private void Update()
    {
        if (isApplingForce) UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        // Calc velocity based on Position
        Vector2 vel = (transform.position - _initialPos) / Time.deltaTime;
        _initialPos = transform.position;

        if (!anim_isAnimating && vel != Vector2.zero) vel = Vector2.zero;

        // Y force is applied based on collisions between platform and player
        _pc.IncreaseExternalVelocity(new Vector2(vel.x, 0));
    }

    /// <summary>
    /// Método chamado ao acabar o movimento da animação
    /// </summary>
    public void EndPlatformMoviment()
    {
        isApplingForce = false;
        _pc.IncreaseExternalVelocity(Vector2.zero);
    }

    public void StartPlatformMoviment()
    {
        isApplingForce = true;
    }
}
