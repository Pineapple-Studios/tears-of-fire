using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatWalkStart : MonoBehaviour, IWalkStart
{
    private bool _isStarted = false;

    public void OnStartWalking()
    {
        if (!_isStarted && transform.localPosition != Vector3.zero)
        {
            GetComponentInChildren<Animator>().Play("clip_idle_walk_transition");
            _isStarted = true;
        }
    }
}
