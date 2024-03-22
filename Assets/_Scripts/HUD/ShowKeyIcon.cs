using UnityEngine;
using UnityEngine.UI;

public class ShowKeyIcon : MonoBehaviour
{
    private Image _img;

    private void Start()
    {
        _img = GetComponent<Image>();
        _img.enabled = false;
    }

    private void Update()
    {
        if (_img.isActiveAndEnabled && !LevelDataManager.Instance.GetKwyRoomKey())
        {
            _img.enabled = false;
        }

        if (!_img.isActiveAndEnabled && LevelDataManager.Instance.GetKwyRoomKey())
        {
            _img.enabled = true;
        }
    }
}
