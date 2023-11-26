using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWalkStart : MonoBehaviour, IWalkStart
{
    [SerializeField]
    private float _delayToStartWalking = 0.5f;

    private bool _isStarted = false;

    public void OnStartWalking()
    {
        if (!_isStarted) StartCoroutine(DelayToStart());
    }

    private IEnumerator DelayToStart()
    {
        yield return new WaitForSeconds(_delayToStartWalking);
        GetComponent<Enemy>().StartWalk();
        _isStarted = true;
    }

    public void ResetWalk()
    {
        _isStarted = false;
    }
}