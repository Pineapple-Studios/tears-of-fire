using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        AMBIENCE,
        FOLEY,
        MUSIC,
        SFX,
        UI,
        VOICE
    }

    [Header("Type")]
    [SerializeField] VolumeType type;

    private Slider _volumeSlider;

    private void Awake()
    {
        _volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (type) 
        {
            case VolumeType.MASTER:
                _volumeSlider.value = FMODAudioManager.Instance.masterVolume;
                break; 
            case VolumeType.AMBIENCE:
                _volumeSlider.value = FMODAudioManager.Instance.ambienceVolume;
                break;
            //case VolumeType.FOLEY:
            //    _volumeSlider.value = FMODAudioManager.Instance.foleyVolume;
            //    break;
            case VolumeType.MUSIC:
                _volumeSlider.value = FMODAudioManager.Instance.musicVolume;
                break;
            case VolumeType.SFX:
                _volumeSlider.value = FMODAudioManager.Instance.sfxVolume;
                break;
            case VolumeType.UI:
                _volumeSlider.value = FMODAudioManager.Instance.uiVolume;
                break;
            case VolumeType.VOICE:
                _volumeSlider.value = FMODAudioManager.Instance.voiceVolume;
                break;
            default:
                Debug.LogWarning("Volume type não suportado" + type);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (type)
        {
            case VolumeType.MASTER:
                FMODAudioManager.Instance.masterVolume = _volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                FMODAudioManager.Instance.ambienceVolume = _volumeSlider.value;
                break;
            //case VolumeType.FOLEY:
            //    FMODAudioManager.Instance.foleyVolume = _volumeSlider.value;
            //    break;
            case VolumeType.MUSIC:
                FMODAudioManager.Instance.musicVolume = _volumeSlider.value;
                break;
            case VolumeType.SFX:
                FMODAudioManager.Instance.sfxVolume = _volumeSlider.value;
                break;
            case VolumeType.UI:
                FMODAudioManager.Instance.uiVolume = _volumeSlider.value;
                break;
            case VolumeType.VOICE:
                FMODAudioManager.Instance.voiceVolume = _volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume type não suportado" + type);
                break;
        }
    }
}
