using FMODUnity;
using FMOD.Studio;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ButtonHoverSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    //[SerializeField]
    //private string _fmodEventName;

    [SerializeField] Button btn;
    [SerializeField] TMP_Dropdown dpd;
    [SerializeField] Slider sld;
    EventInstance moveEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExecuteSFX();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ExecuteSFX();
    }

    private void OnEnable()
    {
        if(btn != null)
        {
            btn.onClick.AddListener(ConfirmSFX);
        }

        if (dpd != null)
        {
            dpd.onValueChanged.AddListener(delegate { ConfirmSFX(); });
        }

        if (sld != null)
        {
            sld.onValueChanged.AddListener(delegate { SliderMove(); });
        }
    }

    private void OnDisable()
    {
        if(btn != null)
        {
            btn.onClick.RemoveListener(ConfirmSFX);
        }

        if (dpd != null)
        {
            dpd.onValueChanged.RemoveListener(delegate { ConfirmSFX(); });
        }

        if (sld != null)
        {
            sld.onValueChanged.RemoveListener(delegate { SliderMove(); });
        }
    }

    private void ExecuteSFX()
    {
        //if (_fmodEventName == null) return;

        moveEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        moveEvent.release();
        //moveEvent = RuntimeManager.CreateInstance(_fmodEventName);
        if (FMODAudioManager.Instance != null && FMODEventsUI.Instance != null) 
            FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.moveUI, this.transform.position);

        moveEvent.start();
    }

    private void ConfirmSFX()
    {
        if (FMODAudioManager.Instance != null && FMODEventsUI.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.clickUI, this.transform.position);
    }

    private void SliderMove()
    {
        FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.sldMove, this.transform.position);
    }
}
