using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private float _cooldownInSeconds = 0.5f;
    [SerializeField]
    private float _dropSpeed = 15;

    [Header("Polling elements")]
    [SerializeField]
    private GameObject _prefabDrop;
    [SerializeField]
    private int _pollingSize;

    private Stack<GameObject> _instancesStack = new Stack<GameObject>();

    private GameObject _tmpElement;

    private void Start()
    {
        // Criando poll
        for (int i = 0; i < _pollingSize; i++)
        {
            _tmpElement = Instantiate(_prefabDrop, _startPoint);
            _tmpElement.SetActive(false);
            _instancesStack.Push(_tmpElement);
        }

        StartCoroutine(StartDroping());
    }

    private IEnumerator StartDroping()
    {
        foreach (var instance in _instancesStack)
        {
            if (instance.activeSelf == false)
            {
                instance.SetActive(true);
                FMODAudioManager.Instance.CreateInstance(FMODEventsTutorial.Instance.leak);
                instance.GetComponent<Drop>().DropSpeed = _dropSpeed;
                break;
            }
        }

        yield return new WaitForSeconds(_cooldownInSeconds);
        StartCoroutine(StartDroping());
    }
}
