using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance;

    private List<EventInstance> eventInstances;

    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume;

    [Range(0, 1)]
    public float ambienceVolume;

    //[Range(0, 1)]
    //public float foleyVolume = 0.6f;

    [Range(0, 1)]
    public float musicVolume;

    [Range(0, 1)]
    public float sfxVolume;

    [Range(0, 1)]
    public float uiVolume;

    [Range(0, 1)]
    public float voiceVolume;

    private Bus _masterBus;
    private Bus _ambienceBus;
    //private Bus _foleyBus;
    private Bus _musicBus;
    private Bus _sfxBus;
    private Bus _uiBus;
    private Bus _voiceBus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        eventInstances = new List<EventInstance>();

        _masterBus = RuntimeManager.GetBus("bus:/");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        //_foleyBus = RuntimeManager.GetBus("bus:/Foley");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _uiBus = RuntimeManager.GetBus("bus:/UI");
        _voiceBus = RuntimeManager.GetBus("bus:/Voice");

    }

    private void Start()
    {
        this.enabled = true;
        SetInitialValues();
    }

    private void Update()
    {
        SaveVolumeToStorage(_masterBus, masterVolume, LocalStorage.GeneralMixerKey());
        SaveVolumeToStorage(_ambienceBus, ambienceVolume, LocalStorage.AmbienceMixerKey());
        //SaveVolumeToStorage(_foleyBus, foleyVolume, LocalStorage.FoleyMixerKey());
        SaveVolumeToStorage(_musicBus, musicVolume, LocalStorage.MusicMixerKey());
        SaveVolumeToStorage(_sfxBus, sfxVolume, LocalStorage.SfxMixerKey());
        SaveVolumeToStorage(_uiBus, uiVolume, LocalStorage.UiMixerKey());
        SaveVolumeToStorage(_voiceBus, voiceVolume, LocalStorage.VoiceMixerKey());
    }

    private void SaveVolumeToStorage(Bus currentBus, float volume, string storageKey)
    {
        currentBus.setVolume(volume);
        LocalStorage.SaveMixerValue(storageKey, volume);
    }

    private const float DEFAULT_VALUE = 0.6f;
    public void SetInitialValues()
    {
        Debug.Log(LocalStorage.GetMixerValue(LocalStorage.GeneralMixerKey(), DEFAULT_VALUE));
        _masterBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.GeneralMixerKey(), DEFAULT_VALUE));
        _ambienceBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.AmbienceMixerKey(), DEFAULT_VALUE));
        //_foleyBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.FoleyMixerKey(), DEFAULT_VALUE));
        _musicBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.MusicMixerKey(), DEFAULT_VALUE));
        _sfxBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.SfxMixerKey(), DEFAULT_VALUE));
        _uiBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.UiMixerKey(), DEFAULT_VALUE));
        _voiceBus.setVolume(LocalStorage.GetMixerValue(LocalStorage.VoiceMixerKey(), DEFAULT_VALUE));
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShot(string sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance PlayOneShotWithParameters(string eventReference, Vector3 position, params (string name, float value)[] parameters)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);

        foreach (var (name, value) in parameters)
        {
            instance.setParameterByName(name, value);
        }

        instance.set3DAttributes(position.To3DAttributes());
        instance.start();
        //instance.release();
        return instance;
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    } 

    public void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
