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
    private GameObject _moveA;
    [SerializeField]
    private GameObject _moveB;
    [SerializeField]
    private GameObject _moveC;
    [SerializeField]
    private GameObject _moveD;

    private List<GameObject> _instancesStack = new List<GameObject>();
    private GameObject _tmpElement;
    private List<Transform> _movePositionGroup = new List<Transform>();
    private int _totalCount = 0;
    private int _currentCount = 0;

    private void Awake()
    {
        SavePositions(_moveA, _movePositionGroup);
        SavePositions(_moveB, _movePositionGroup);
        SavePositions(_moveC, _movePositionGroup);
        SavePositions(_moveD, _movePositionGroup);
    }

    private void Start()
    {
        _totalCount = _movePositionGroup.Count;

        for (int i = 0; i < _pollingSize; i++)
        {
            _tmpElement = Instantiate(_prefabDrop, transform);
            _tmpElement.SetActive(false);
            _instancesStack.Add(_tmpElement);
        }
    }

    private void SavePositions(GameObject moviment, List<Transform> list)
    {
        DropPos[] _posList = moviment.GetComponentsInChildren<DropPos>();
        List<Transform> _transList = new List<Transform>();
        foreach (DropPos drop in _posList) _transList.Add(drop.transform);
        foreach (Transform trans in _transList) list.Add(trans);
    }

    public void StartDroping()
    {
        GameObject _drop = _instancesStack.FirstOrDefault(x => !x.activeSelf);
        _drop.transform.position = _movePositionGroup[_currentCount].position;
        _drop.SetActive(true);

        if (_currentCount < _totalCount - 1) _currentCount++;
        else _currentCount = 0;
    }
}
