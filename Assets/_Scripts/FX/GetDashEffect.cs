using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDashEffect : MonoBehaviour
{
    [Header("Global Positions")]
    [SerializeField]
    private Transform _origin;
    [SerializeField]
    private Transform _centerTarget;
    [SerializeField]
    private Transform _gun;
    [SerializeField]
    private Transform _player;

    [Header("Particles Systems")]
    [SerializeField]
    private ParticleSystem _psEmitter;
    [SerializeField]
    private ParticleSystem _psAccumulator;
    [SerializeField]
    private ParticleSystem _psGun;

    private void Start()
    {
        SetParticleDeadTransform();
        StopAll();
    }

    void Update()
    {
        LookAtTargets();
    }

    private void StopAll()
    {
        _psEmitter.Stop();
        _psAccumulator.Stop();
        _psGun.Stop();
    }

    public void LookAtTargets()
    {
        // 2D lookat
        _origin.right = _centerTarget.position - _origin.position;
        // 2D lookat
        if (_player != null) _gun.right = _player.position - _gun.position;
    }

    public void SetParticleDeadTransform()
    {
        if (_player != null) _psGun.collision.AddPlane(_player);
    }

    public void StartAll()
    {
        _psEmitter.Play();
        _psAccumulator.Play();
        _psGun.Play();
    }

    public void SetPlayerTransform(Transform newTrans)
    {
        _player = newTrans;
    }
}
