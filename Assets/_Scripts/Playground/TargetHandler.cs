using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> _pos = new(10);
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Animator _anim;

    private int _currentPos = 0;

    public void Initiate()
    {
        _currentPos = 0;
        gameObject.transform.position = _pos[_currentPos] + _offset;
        _anim.SetTrigger("StartFlicking");
    }

    public void NextPostion()
    {
        _currentPos++;
        gameObject.transform.position = _pos[_currentPos] + _offset;
        _anim.SetTrigger("StartFlicking");
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
