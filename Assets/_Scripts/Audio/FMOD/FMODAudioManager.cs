using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.Rendering;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance;

    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume = 0.6f;

    [Range(0, 1)]
    public float ambienceVolume = 0.6f;

    [Range(0, 1)]
    public float foleyVolume = 0.6f;

    [Range(0, 1)]
    public float musicVolume = 0.6f;

    [Range(0, 1)]
    public float sfxVolume = 0.6f;

    [Range(0, 1)]
    public float uiVolume = 0.6f;

    [Range(0, 1)]
    public float voiceVolume = 0.6f;

    private Bus _masterBus;
    private Bus _ambienceBus;
    private Bus _foleyBus;
    private Bus _musicBus;
    private Bus _sfxBus;
    private Bus _uiBus;
    private Bus _voiceBus;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um Audio Manager");
        }

        Instance = this;

        _masterBus = RuntimeManager.GetBus("bus:/");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        _foleyBus = RuntimeManager.GetBus("bus:/Foley");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _uiBus = RuntimeManager.GetBus("bus:/UI");
        _voiceBus = RuntimeManager.GetBus("bus:/Voice");
    }

    private void Start()
    {
    }

    private void Update()
    {
        _masterBus.setVolume(masterVolume);
        _ambienceBus.setVolume(ambienceVolume);
        _foleyBus.setVolume(foleyVolume);
        _musicBus.setVolume(musicVolume);
        _sfxBus.setVolume(sfxVolume);
        _uiBus.setVolume(uiVolume);
        _voiceBus.setVolume(voiceVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
