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
    private Collider2D _collider;

    private void Start()
    {
        _an = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            // Debug.Log(collision.gameObject.name);
            PlayerProps _pp = collision.gameObject.GetComponentInChildren<PlayerProps>();
            if (_pp == null) return;
            if (_pp.IsFullLife())
            {
                OnAnimationCannotGetLife();
                return;
            }
            
            _pp.HealLife(_heallingLife);

            OnAnimationGetLife();
        }
    }

    private void OnAnimationGetLife()
    {
        _an.SetTrigger("OnCollision");
        _collider.enabled = false;
        FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.catchLifeItem, this.transform.position);
    }

    private void OnAnimationCannotGetLife()
    {
        _an.SetTrigger("OnFullLife");
        FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.denyLifeItem, this.transform.position);
    }
}
