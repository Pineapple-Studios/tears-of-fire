using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumperDefiningDirection : MonoBehaviour
{
    [Header("Properties")]
    [Tooltip("In seconds")]
    [SerializeField]
    private float _totalMovimentDuration = 5f;

    [Header("Casting props")]
    [SerializeField]
    private float _castRadius = 5f;
    [SerializeField]
    private LayerMask _playerMask;
    [SerializeField]
    private LayerMask _groundMask;

    private CircleCollider2D _circleCollider;
    private bool _isGrounded = false;
    private int _xDir;
    private JumperV2 _jv2;
    private float _timer;
    private bool _runTimer = false;

    private void Awake()
    {
        _circleCollider = transform.AddComponent<CircleCollider2D>();
        _circleCollider.radius = _castRadius;
        _circleCollider.isTrigger = true;
    }

    private void Start()
    {
        _jv2 = GetComponent<JumperV2>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerMask) != 0 && _isGrounded && !_jv2.IsEventsEnabled())
        {
            // Definindo direção do movimento horizontal
            float direction = (transform.position - collision.gameObject.transform.position).normalized.x;
            _xDir = direction < 0 ? 1 : -1;
            // Definindo rotação de acordo com o elemento colidido
            transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, direction < 0 ? 0 : 180, 0);
            // Ativando movimento
            _jv2.EnableEvents(_xDir);

            _runTimer = true;
        }
    }

    private void Update()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _groundMask);

        if (_runTimer) _timer += Time.deltaTime;

        if (_timer >= _totalMovimentDuration)
        {
            _jv2.DisableEvents();
            _timer = 0f;
            _runTimer = false;
        }
    }
}
