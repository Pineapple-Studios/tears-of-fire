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

    private bool _isGoingToSomewhere = false;

    private void Update()
    {
        if (EndPoint == null || StartPoint == null) return;

        if (!_isGoAndBack)
        {
            // Entou mais a direita do ponto final
            if (transform.position.x >= EndPoint.transform.position.x)
            {
                GoToLeft();
                if (transform.position.x <= EndPoint.transform.position.x)
                {
                    RestartElement();
                }
            }

            // Entou mais a esquerda do ponto final
            if (transform.position.x <= EndPoint.transform.position.x)
            {
                GoToRight();
                if (transform.position.x >= EndPoint.transform.position.x)
                {
                    RestartElement();
                }
            }
        }
        else
        {
            if (transform.position == EndPoint.transform.position)
            {
                _isGoingToSomewhere = false;
                Flip();
            }
            if (transform.position == StartPoint.transform.position)
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
            if (transform.rotation.y == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        } 
        else
        {
            if (transform.rotation.x == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(180, 0, 0);
            }
        }
    }

    private void GoToLeft()
    {
        transform.position = Vector3.MoveTowards(transform.position, EndPoint.position, _timeToTravel * Time.deltaTime);
    }

    private void GoToRight()
    {
        transform.position = Vector3.MoveTowards(EndPoint.position, transform.position, _timeToTravel * Time.deltaTime);
    }

    private void RestartElement()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = StartPoint.position;
    }

    private void GoToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, StartPoint.position, _timeToTravel * Time.deltaTime);
    }

    private void GoToEnd()
    {
        transform.position = Vector3.MoveTowards(transform.position, EndPoint.position, _timeToTravel * Time.deltaTime);
    }
}
