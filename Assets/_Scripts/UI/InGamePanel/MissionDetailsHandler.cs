using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetailsHandler : MonoBehaviour
{
    [SerializeField]
    private Image _thumbnail;
    [SerializeField]
    private TMP_Text _title;
    [SerializeField]
    private TMP_Text _description;

    private void OnEnable()
    {
        MissionButton.OnFocusMissionElement += RenderData;
    }

    private void OnDisable()
    {
        MissionButton.OnFocusMissionElement -= RenderData;
    }

    private void RenderData(string title, string description, Sprite image, bool isCompleted)
    {
        _thumbnail.sprite = image;
        _title.text = title;
        _description.text = description;
    }
}
