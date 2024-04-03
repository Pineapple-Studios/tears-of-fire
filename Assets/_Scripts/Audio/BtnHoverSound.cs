using FMODUnity;
using FMOD.Studio;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonHoverSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField]
    private string _fmodEventName;
    
    EventInstance moveEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExecuteSFX();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ExecuteSFX();
    }

    private void ExecuteSFX()
    {
        if (_fmodEventName == null) return;

        moveEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        moveEvent.release();
        moveEvent = RuntimeManager.CreateInstance(_fmodEventName);
        moveEvent.start();
    }
}
