using FMOD.Studio;
using UnityEngine;

public class FMODVolumeController : MonoBehaviour
{
    private const string GROUP_NAME = "bus:/Music";

    [SerializeField]
    [Range(-80f, 10f)]
    private float busVolume;

    private Bus bus;
    private float volume;

    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus(GROUP_NAME);
    }

    void Update()
    {
        volume = Mathf.Pow(10.0f, busVolume / 20f);
        bus.setVolume(volume);
    }
}
