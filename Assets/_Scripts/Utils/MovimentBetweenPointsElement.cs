using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentBetweenPointsElement : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    [SerializeField]
    private float _timeToTravel;
    [SerializeField]
    private bool _isGoAndBack = false;
    [SerializeField]
    private bool _isHorizontalMoviment = true;
    [SerializeField]
    private bool _isManagingParent = false;

    private bool _isGoingToSomewhere = false;
    private Transform _activeTransform = null;

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

    private void Update()
    {
        if (EndPoint == null || StartPoint == null) return;

        if (!_isGoAndBack)
        {
            // Entou mais a direita do ponto final
            if (_activeTransform.position.x >= EndPoint.transform.position.x)
            {
                GoToLeft();
                if (_activeTransform.position.x <= EndPoint.transform.position.x)
                {
                    RestartElement();
                }
            }

            // Entou mais a esquerda do ponto final
            if (_activeTransform.position.x <= EndPoint.transform.position.x)
            {
                GoToRight();
                if (_activeTransform.position.x >= EndPoint.transform.position.x)
                {
                    RestartElement();
                }
            }
        }
        else
        {
            if (_activeTransform.position == EndPoint.transform.position)
            {
                _isGoingToSomewhere = false;
                Flip();
            }
            if (_activeTransform.position == StartPoint.transform.position)
            {
                _isGoingToSomewhere = true;
                Flip();
            }
            
            if (!_isGoingToSomewhere) GoToStart();
            else GoToEnd();
        }
    }

    private void Flip()
    {
        if (_isHorizontalMoviment)
        {
            if (_activeTransform.rotation.y == 1)
            {
                _activeTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _activeTransform.rotation = Quaternion.Euler(0, 180, 0);
            }
        } 
        else
        {
            if (_activeTransform.rotation.x == 1)
            {
                _activeTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _activeTransform.rotation = Quaternion.Euler(180, 0, 0);
            }
        }
    }

    private void GoToLeft()
    {
        _activeTransform.position = Vector3.MoveTowards(_activeTransform.position, EndPoint.position, _timeToTravel * Time.deltaTime);
    }

    private void GoToRight()
    {
        _activeTransform.position = Vector3.MoveTowards(EndPoint.position, _activeTransform.position, _timeToTravel * Time.deltaTime);
    }

    private void RestartElement()
    {
        _activeTransform.gameObject.SetActive(false);
        _activeTransform.position = StartPoint.position;
    }

    private void GoToStart()
    {
        _activeTransform.position = Vector3.MoveTowards(_activeTransform.position, StartPoint.position, _timeToTravel * Time.deltaTime);
    }

    private void GoToEnd()
    {
        _activeTransform.position = Vector3.MoveTowards(_activeTransform.position, EndPoint.position, _timeToTravel * Time.deltaTime);
    }
}
