using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : MonoBehaviour
{
    [Header("Props")]
    [SerializeField]
    private float _heallingLife = 20f;

    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;

    private Animator _an;

    private void Start()
    {
        _an = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            Debug.Log(collision.gameObject.name);
            PlayerProps _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp == null) return;
            if (_pp.IsFullLife()) return;
            
            _pp.HealLife(_heallingLife);

            OnAnimation();
        }
    }

    void OnAnimation()
    {
        _an.SetTrigger("OnCollision");
        Debug.Log("Trigger");
        //an.Play("clip_Life");
        //Debug.Log("Clip");
    }
}
