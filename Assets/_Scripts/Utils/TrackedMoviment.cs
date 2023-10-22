using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TrackedMoviment : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _pointsToNavigate = new List<Transform> { };
    [SerializeField]
    private float _timeToTravel;
    [SerializeField]
    private bool _isManagingParent = false;

    private Transform _activeTransform = null;
    private int _counter = 0;

    private void Start()
    {
        if (_isManagingParent)
        {
            // Utilizando o transform do elemento pai
            _activeTransform = transform.parent.transform;
        }
        else
        {
            _activeTransform = transform;
        }
    }

    void Update()
    {
        if (_activeTransform.position == _pointsToNavigate[_counter].position)
        {
            _counter = _counter >= _pointsToNavigate.Count - 1 ? 0 : _counter + 1;
        }

        GoToNext();        
    }

    private void GoToNext()
    {
        _activeTransform.position = Vector3.MoveTowards(
            _activeTransform.position, 
            _pointsToNavigate[_counter].position, 
            _timeToTravel * Time.deltaTime
        );
    }
}
