using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleComponentByLayerPresence : MonoBehaviour
{
    [Header("Casting props")]
    [SerializeField]
    private float _castRadius = 5f;
    [SerializeField]
    private LayerMask _layerPresenceMask;

    [Header("Component props")]
    [SerializeField]
    private MonoBehaviour _componentToToggle;

    private CircleCollider2D _circleCollider;
    private bool _triggerOnce = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _castRadius);
    }

    private void Awake()
    {
        _circleCollider = transform.AddComponent<CircleCollider2D>();
        _circleCollider.radius = _castRadius;
        _circleCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _layerPresenceMask) != 0) && _triggerOnce == false)
        {
            _componentToToggle.enabled = !_componentToToggle.enabled;
            _triggerOnce = true;
        }
    }

    public void ResetState()
    {
        _triggerOnce = false;
    }
}
