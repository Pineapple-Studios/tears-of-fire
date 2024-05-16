using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    [SerializeField] TMP_Text valueTxt;
    [SerializeField] Slider brightnessSld;
    [SerializeField] PostProcessProfile profile;
    [SerializeField] PostProcessLayer layer;

    AutoExposure exposure;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(brightnessSld.gameObject);
        profile.TryGetSettings(out exposure);
        float brightValue = LocalStorage.GetBright(1f);
        float brightValueUpdated = this.OnChangeValue(brightValue);
        LocalStorage.SetBright(brightValueUpdated);
        brightnessSld.SetValueWithoutNotify(brightValue);
        //valueTxt.text = brightValueUpdated.ToString(@"0.0");
    }

    public void AdjustBrightness(float value)
    {
        if (value != 0)
        {
            LocalStorage.SetBright(value);
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = 0.2f;
        }
        //valueTxt.text = value.ToString(@"0.0");
    }

    private float OnChangeValue(float value)
    {
        exposure.keyValue.value = value;
        return value;
    }


}
