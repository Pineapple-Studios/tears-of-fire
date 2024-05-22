using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour, ISelectHandler
{
    public static Action<string, string, Sprite, bool> OnFocusMissionElement;

    public string MissionTitle = "Test";
    public string ShortDescription = "Test";
    [TextArea]
    public string FullDescription = "Test";
    public Sprite Image;
    public Sprite Thumbnail;

    private bool _isCompleted = false;

    private Image _fieldImage;
    private TMP_Text _fieldTitle;
    private TMP_Text _fieldDescription;

    public void OnSelect(BaseEventData eventData)
    {
        OnFocusMissionElement(
            MissionTitle,
            FullDescription,
            Image,
            _isCompleted
        );
    }

    public void InitializeButton()
    {
        Image[] imgs = GetComponentsInChildren<Image>();

        if (imgs.Length > 0)
        {
            _fieldImage = imgs[0].name == "img_check" ? imgs[0] : imgs[1];
            _fieldImage.sprite = Thumbnail;
        }

        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();

        _fieldTitle = texts[0].name == "lbl_title" ? texts[0] : texts[1];
        _fieldTitle.text = MissionTitle;

        _fieldDescription = texts[0].name == "lbl_title" ? texts[1] : texts[0];
        _fieldDescription.text = ShortDescription;
    }

    public bool IsCompleted() => _isCompleted;

    public void SetIsCompleted(bool state) {
        _isCompleted = state;
    } 
}
