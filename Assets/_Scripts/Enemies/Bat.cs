using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Bat : MonoBehaviour
{
    [Header("Casting props")]
    [SerializeField]
    private float _rangeToBullet = 5f;
    [SerializeField]
    private string _targetComponent;

    [Header("Bullet props")]
    [SerializeField]
    private float _reloadTime = 2f;
    [SerializeField]
    private BatBullet _bulletPrefab;
    [SerializeField]
    private LayerMask _bulletTarget;

    private GameObject _target;
    private bool _isShooting = false;
    private Enemy _enemy;

    private void Start()
    {
        _target = GameObject.Find(_targetComponent);
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (_target == null) return;

        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (dist <= _rangeToBullet * 2 && !_isShooting)
        {
            _isShooting = true;
            StartCoroutine(ShootRoutine());
        }
    }
    
    private void Shoot()
    {
        BatBullet obj = Instantiate(_bulletPrefab, transform);
        obj.SetTarget(_target.transform, _bulletTarget, _enemy.GetDamagePoints());
    }

    private IEnumerator ShootRoutine()
    {
        Shoot();
        yield return new WaitForSeconds(_reloadTime);
        _isShooting = false;
    }
    


}