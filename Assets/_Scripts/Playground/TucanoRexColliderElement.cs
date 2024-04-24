using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TucanoRexColliderElement : MonoBehaviour
{
    [SerializeField]
    private TucanoRex _tr;
    [SerializeField]
    private TucanoRexProps _trp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tr.TriggerCollider(collision);
        _trp.TriggerCollider(collision);
    }
}
