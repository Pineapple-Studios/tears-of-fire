using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PollingDrops : MonoBehaviour
{
    [Header("Polling elements")]
    [SerializeField]
    private GameObject _prefabDrop;
    [SerializeField]
    private int _pollingSize;

    [Header("Positioning")]
    [SerializeField]
    private Transform _instantiatePosition;
    [SerializeField]
    private Vector3 _offsetPosition;

    private List<GameObject> _instancesStack = new List<GameObject>();
    private GameObject _tmpElement;

    private void Start()
    {
        for (int i = 0; i < _pollingSize; i++)
        {
            _tmpElement = Instantiate(_prefabDrop, transform);
            _tmpElement.SetActive(false);
            _instancesStack.Add(_tmpElement);
        }
    }

    public void StartDroping()
    {
        GameObject _drop = _instancesStack.FirstOrDefault(x => !x.activeSelf);
        _drop.transform.position = _instantiatePosition.position + _offsetPosition;
        _drop.SetActive(true);
    }
}
