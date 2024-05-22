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
        btn.onClick.AddListener(ConfirmSFX);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(ConfirmSFX);
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
}
