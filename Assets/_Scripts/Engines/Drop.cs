using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private LayerMask _target;

    private float _speed;
    private float _damage;

    private Transform _root;

    private void Start()
    {
        _root = transform.parent.gameObject.transform;
        _root.position = Vector3.zero;
    }

    private void Update()
    {
        _root.position += new Vector3(_root.position.x, _root.position.y + (_speed * Time.deltaTime),1);
    }

    public void Init(float speed, float damage)
    {
        _damage = damage;
        _speed = speed; 
    }
}
