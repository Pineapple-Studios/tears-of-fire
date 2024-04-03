using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock: MonoBehaviour
{
    [Header("Props")]
    [SerializeField]
    private int _hitsToBreak = 3;
    [SerializeField]
    private List<GameObject> _blockDepedencies = new List<GameObject> { };
    [SerializeField]
    private ParticleSystem _particleSystemRight;
    [SerializeField]
    private ParticleSystem _particleSystemLeft;

    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerMask;
    enum Directions { Up, Down, Left, Right };
    [SerializeField]
    private Directions _breackableDirections;
    [SerializeField]
    private float _distanceToCheckPlayer = 4f;

    private BoxCollider2D _col;
    private int _counter = 0;
    private Vector2[] _breackableDirectionsVectors = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(
            transform.position, 
            new Vector2(transform.position.x, transform.position.y) + _breackableDirectionsVectors[(int) _breackableDirections] * _distanceToCheckPlayer);

        Gizmos.DrawWireSphere(
            transform.position,
            _distanceToCheckPlayer
        );
    }

    private void Start()
    {
        SetColliderAccordingOfSprite();
    }

    /// <summary>
    /// Executa um hit no bloco quebrável e verifica se devemos destruílo
    /// </summary>
    public void HitWall()
    {
        bool isOnRightDirection = false;
        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position, 
            _distanceToCheckPlayer, 
            _breackableDirectionsVectors[(int)_breackableDirections],
            _distanceToCheckPlayer,
            _playerMask
        );
        isOnRightDirection = hit.collider != null;
        if (!isOnRightDirection) return;

        // Debug.Log("HitRight");

        _counter++;
        StoneParticle();
        RumbleManager.instance.RumblePulse(0.25f, 1f, 0.25f);

        if (_counter == _hitsToBreak)
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            DestroyDependencies();
            Destroy(parent);
        }
    }

    /// <summary>
    /// Destroi a lista de elementos na dependencia desse bloco
    /// </summary>
    private void DestroyDependencies()
    {
        if (_blockDepedencies.Count == 0) return;

        foreach(GameObject obj in _blockDepedencies)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// Busca o elemento que tem o sprite e adiciona o collider para que o collider
    /// possua o mesmo tamanho do sprite exibido;
    /// </summary>
    private void SetColliderAccordingOfSprite()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        SpriteRenderer spriteRenderer = parent.GetComponentInChildren<SpriteRenderer>();
        GameObject element = spriteRenderer.gameObject;
        _col = element.AddComponent<BoxCollider2D>();
    }

    void StoneParticle()
    {

        if(_breackableDirections == Directions.Left)
        _particleSystemLeft.Play();
        else if(_breackableDirections == Directions.Right) _particleSystemRight.Play();
    }
}
