using UnityEngine;

public class BatAnimatorEvents : MonoBehaviour
{
    private GameObject _parent;
    private Bat _bat;

    void Start()
    {
        _parent = gameObject.transform.parent.gameObject;
        _bat = _parent.GetComponentInChildren<Bat>();    
    }

    public void TriggerShootParticleEvent()
    {
        _bat.InstantiateShoot();
    }
}
