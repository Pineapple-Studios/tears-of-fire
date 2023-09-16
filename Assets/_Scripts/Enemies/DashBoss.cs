using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoss : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private GameObject _player;

    private Transform _trans;

    private void Start()
    {
        _trans = transform.parent.transform;
    }

    private void Update()
    {
        LookAt2D(_trans, _player.transform);
    }

    private void LookAt2D(Transform obj, Transform target)
    {
        Quaternion rotation =
            Quaternion.LookRotation(target.transform.position - obj.position, obj.TransformDirection(Vector3.up));
        obj.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }
}
