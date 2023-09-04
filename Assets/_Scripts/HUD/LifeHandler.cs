using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LifeHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("GameObject que representa uma vida")]
    private GameObject _life;
    [SerializeField]
    [Tooltip("Player")]
    private PlayerProps _playerProps;
    [SerializeField]
    [Tooltip("Unidade de vida")]
    private float _lifeUnit = 20f;

    private List<GameObject> _lifeList = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _playerProps.GetLife() / _lifeUnit; i++)
        {
            _lifeList.Add(Instantiate(_life, transform));
        }
    }

    private void OnEnable()
    {
        PlayerProps.onChangePlayerLife += UpdateLifeDisplayed;
    }

    private void OnDisable()
    {
        PlayerProps.onChangePlayerLife -= UpdateLifeDisplayed;
    }

    private void UpdateLifeDisplayed(float obj)
    {
        _lifeList.Clear();
        Transform[] tList = gameObject.GetComponentsInChildren<Transform>();
        // Começa em um para remover o Transform do próprio objeto
        for (int j = 1; j < tList.Length; j++)
        {
            Destroy(tList[j].gameObject);
        }

        for (int i = 0; i < obj / _lifeUnit; i++)
        {
            _lifeList.Add(Instantiate(_life, transform));
        }
    }
}

