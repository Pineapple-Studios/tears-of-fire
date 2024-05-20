using TMPro;
using UnityEngine;

public class AchievementHUDHandler : MonoBehaviour
{
    private const string TOAST_ANIM = "clip_show_toast";

    [SerializeField]
    private TMP_Text _content;
    [SerializeField]
    private TMP_Text _title;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void ShowToast(string title, string message)
    {
        _title.text = title;
        _content.text = message;
        _anim.Play(TOAST_ANIM);
    }
}
