using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RiverElement : MonoBehaviour
{
    [Header("River size")]
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _endPoint;
    [SerializeField]
    private float _distanceBetweenElements;
    [SerializeField]
    private float _timeToCrossRiver;

    [Header("Polling elements")]
    [SerializeField]
    private GameObject _prefabElementToInstance;
    [SerializeField]
    private int _pollingSize;

    private Stack<ElementAtPooling> instancesStack = new Stack<ElementAtPooling>();
    private GameObject _tmpElement;
    private GameObject _lastElement;

    private struct ElementAtPooling
    {
        public GameObject Element { get; set; }
        public float PercentageToCompleteThePath { get; set; }

        public ElementAtPooling(GameObject go, float time)
        {
            Element = go;
            PercentageToCompleteThePath = time;
        }

        public void IncreaseTime(float time)
        {
            PercentageToCompleteThePath += time;
        }
    }

    private void Start()
    {
        // Criando poll
        for (int i = 0; i < _pollingSize; i++)
        {
            _tmpElement = Instantiate(_prefabElementToInstance, _startPoint);
            _tmpElement.gameObject.GetComponent<MovimentBetweenPointsElement>().EndPoint = _endPoint;
            _tmpElement.gameObject.GetComponent<MovimentBetweenPointsElement>().StartPoint = _startPoint;
            instancesStack.Push(new ElementAtPooling(_tmpElement, 0));
            _tmpElement.SetActive(false);
        }

        ActiveElementOnRiver();
    }

    private void Update()
    {
        ShouldAddNewElement();
    }

    private void ShouldAddNewElement()
    {
        if (
            _lastElement == null || 
            Mathf.Abs(_lastElement.transform.position.x - _startPoint.position.x) < _distanceBetweenElements
            ) return;

        ActiveElementOnRiver();
    }

    private void ActiveElementOnRiver()
    {
        GameObject element;

        foreach(var instance in instancesStack)
        {
            GameObject el = instance.Element;
            if (el.activeSelf == false)
            {
                element = el;
                element.SetActive(true);
                _lastElement = element;
                return;
            }
        }

    }
}
