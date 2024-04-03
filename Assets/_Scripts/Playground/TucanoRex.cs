using TMPro;
using UnityEngine;

public class TucanoRex : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField]
    private TMP_Text _lblLifeBoss;

    [Header("References")]
    [SerializeField]
    private Animator _anim;

    [Header("Controllers")]
    [SerializeField]
    private LayerMask _wallLayer;

    private TucanoRexProps _trp;
    private int _tmpAnimLife = 0;
    private PlayerController _playerController;
    private bool _isStarted = false;

    private void Awake()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        _trp = GetComponent<TucanoRexProps>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignorando colisoes com as paredes
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<CompositeCollider2D>());
        }

        // Ignorando colisoes entre elementos com a mesma tag
        if (((1 << collision.gameObject.layer) & gameObject.layer) != 0)
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<Collider2D>());
        }
    }

    private void Update()
    {
        // Update state on Animator
        _tmpAnimLife = (int) _trp.GetCurrentLife();
        if (GetCurrentLife() != _tmpAnimLife) UpdateLife(_tmpAnimLife);
    }

    private int GetCurrentLife()
    {
        return _anim.GetInteger("bossLife"); 
    }

    private void UpdateLife(int life)
    {
        if (_lblLifeBoss != null) _lblLifeBoss.text = life.ToString();
        _anim.SetInteger("bossLife", life);
    }

    public void StartTucanoRex()
    {
        _playerController.FreezeMovement();
        _playerController.DisableInput();
        _anim.SetBool("isStarted", true);
        _isStarted = true;
    }

    public bool IsStarted() => _isStarted;

}
