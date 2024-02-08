using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticHitElement : MonoBehaviour
{
    [SerializeField]
    private MagneticHandler _mh;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void OnNext()
    {
        _anim.SetTrigger("hasHitted");
        _mh.OnNextStep();
    }
}
