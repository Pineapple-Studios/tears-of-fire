using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProps : MonoBehaviour
{
    [SerializeField]
    private float _initialLife = 600f;

    private float _currentLife;
    private Tears _currentTear = Tears.faseOne;

    private void Start()
    {
        _currentLife = _initialLife;
    }

    public void HasDamaged(float damage)
    {
        _currentLife -= damage;
        HandleTears();
    }

    private void HandleTears()
    {   
        if (_currentLife > (int) Tears.faseOne )
        {
            _currentTear = Tears.faseOne;
            return;
        }

        if (_currentLife > (int) Tears.faseTwo)
        {
            _currentTear = Tears.faseTwo;
            return;
        }

        if (_currentLife > (int) Tears.faseThree)
        {
            _currentTear = Tears.faseThree;
            return;
        }

        _currentTear = Tears.faseFour;
    }

    public float GetLife()
    {
        return _currentLife;
    }

    public Tears GetCurrentTear()
    {
        return _currentTear;
    }
}

public enum Tears
{
    faseOne = 400,
    faseTwo = 300,
    faseThree = 200,
    faseFour = 0
}