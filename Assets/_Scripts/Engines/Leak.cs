using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private Transform _start;
    [SerializeField]
    private Transform _end;

    [Header("Drop")]
    [SerializeField]
    private GameObject _dropPrefab;
    [SerializeField]
    private float _dropSpeed;
    [SerializeField]
    private float _respownTime;
    [SerializeField]
    private float _damage = 20;


    private void Start()
    {
        StartCoroutine(EmitDrop());
    }

    private IEnumerator EmitDrop()
    {
        GameObject obj = Instantiate(_dropPrefab, _start);
        obj.transform.parent = null;
        obj.transform.localScale = Vector3.one;
        Drop _dp = obj.GetComponentInChildren<Drop>();
        _dp.Init(_dropSpeed, _damage);
        yield return new WaitForSeconds(_respownTime);
        StartCoroutine(EmitDrop());
    }
}
