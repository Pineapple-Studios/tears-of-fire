using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonHoverSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private string soundName = "Move";

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.Play(soundName);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.Play(soundName);
    }
}
