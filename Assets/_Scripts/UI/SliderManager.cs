using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SliderManager : MonoBehaviour
{
    [Header("Slider")]
    //[SerializeField]
    public GameObject bar;
    public float maxValue;
    public float currentValue;

    [Header("Actions")]
    [SerializeField] public InputActionAsset Actions;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = maxValue;
    }

    void Update()
    {
        bar.transform.localScale = new Vector3(currentValue / maxValue, bar.transform.localScale.y, bar.transform.localScale.z);
    }

}
