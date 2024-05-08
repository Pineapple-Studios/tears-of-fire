using UnityEngine;

public class VFXPuff : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _bigPuff;
    [SerializeField]
    private ParticleSystem _littlePuff;

    public void PlayPuff()
    {
        Debug.Log("PlayPuff");
        _bigPuff.Play();
        _littlePuff.Play();
    }
}
