using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private const string BAT_SHOOT_EVENT = "event:/Tutorial/Enemy/Bat/SFX_Atk";

    [Header("Casting props")]
    [SerializeField]
    private float _rangeToBullet = 5f;
    [SerializeField]
    private string _targetComponent;
    [SerializeField]
    private Animator _anim;


    [Header("Bullet props")]
    [SerializeField]
    private float _reloadTime = 2f;
    [SerializeField]
    private BatBullet _bulletPrefab;
    [SerializeField]
    private LayerMask _bulletTarget;

    private GameObject _target;
    private Enemy _enemy;
    private SpriteRenderer _sprite;
    private bool _isShooting = false;
    private bool _canShoot = false;
    private bool _targetIsOnRight = false;

    private void Start()
    {
        _target = GameObject.Find(_targetComponent);
        _enemy = GetComponentInParent<Enemy>();
        _sprite = gameObject.transform.parent.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnBecameVisible()
    {
        StartCoroutine("CanShotActiveCoroutine");
    }

    private IEnumerator CanShotActiveCoroutine()
    {
        yield return new WaitForSeconds(1f);
        _canShoot = true;
    }

    private void OnBecameInvisible()
    {
        _canShoot = false;
    }

    private void Update()
    {
        if (_target == null) return;
        HandleTargetPosition();
        if (!_canShoot) return;

        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (dist <= _rangeToBullet * 2 && !_isShooting)
        {
            _isShooting = true;
            StartCoroutine(ShootRoutine());
        }
    }
    
    private void Shoot()
    {
        _anim.Play("clip_attack");
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(BAT_SHOOT_EVENT, this.transform.position);
    }

    private IEnumerator ShootRoutine()
    {
        Shoot();
        yield return new WaitForSeconds(_reloadTime);
        _isShooting = false;
    }

    private void HandleTargetPosition()
    {
        Vector3 isa = transform.position - _target.transform.position;
        _targetIsOnRight = isa.x < 0;
        _sprite.flipX = _targetIsOnRight;
    }

    public void ResetState()
    {
        _isShooting = false;
        _canShoot = false;
    }

    public void InstantiateShoot()
    {
        BatBullet obj = Instantiate(_bulletPrefab, transform);
        obj.SetTarget(_target.transform, _bulletTarget, _enemy.GetDamagePoints());
    }
}
