using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MagneticPuzzle : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField]
    private GameObject _platform;
    [SerializeField]
    private List<GameObject> _anchorPoints = new List<GameObject> {};

    [Header("Props")]
    [SerializeField]
    private float _velocityPoints = 2f;
    [SerializeField]
    private float _timeToGoBack = 2f;

    private int _currentHook = 0;
    private bool _inMoviment = false;
    private bool _isGoingBack = false;

    private void Start()
    {
        SetInitialHookPosition();
    }

    private void Update()
    {
        if (_inMoviment) GoAhead();
        if (_isGoingBack) GoBack();
    }

    private void SetInitialHookPosition()
    {
        if (_anchorPoints.Count == 0 || _platform == null) return;

        _platform.transform.position = _anchorPoints[0].transform.position;
        _currentHook++;
    }

    private void GoAhead()
    {
        _platform.transform.position = Vector3.MoveTowards(
            _platform.transform.position,
            _anchorPoints[_currentHook].transform.position,
            _velocityPoints * Time.deltaTime
        );

        if (
            _currentHook < _anchorPoints.Count && 
            _platform.transform.position == _anchorPoints[_currentHook].transform.position
        )
        {
            Debug.Log("Start time count");
            _inMoviment = false;
            _currentHook++;
            StartCoroutine(CounterToBackStep());
        }
    }

    private IEnumerator CounterToBackStep()
    {
        yield return new WaitForSeconds(_timeToGoBack);
        _currentHook--;
        _isGoingBack = true;
    }

    private void GoBack()
    {
        _platform.transform.position = Vector3.MoveTowards(
            _anchorPoints[_currentHook].transform.position,
            _platform.transform.position,
            _velocityPoints * Time.deltaTime
        );

        if (_platform.transform.position == _anchorPoints[_currentHook].transform.position)
        {
            _isGoingBack = false;
        }
    }

    public void GoToNextPoint()
    {
        // Bloqueandoa a��es caso a plataforma estaja em movimento contr�rio
        if (_isGoingBack) return;

        if (_anchorPoints.Count != _currentHook) _inMoviment = true;
    }
}
