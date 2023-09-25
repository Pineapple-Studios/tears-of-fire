using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ScenarioColorManager : MonoBehaviour
{
    public static ScenarioColorManager Instance { get; private set; }

    [Header("Inputs")]
    [SerializeField]
    private Transform _followTo;
    [SerializeField]
    private GameObject _spotLight;
    [SerializeField]
    private GameObject _directionalLight;
    [SerializeField]
    private GameObject _pointLight;

    [Header("Environment")]
    [SerializeField]
    private Color _color;

    [Header("Ambience")]
    [SerializeField]
    private float _ambienceRange;
    [SerializeField]
    private Color _ambienceColor;
    [SerializeField]
    private float _ambienceIntensity;

    [Header("Character Glow")]
    [SerializeField]
    private float _characterGlowRange;
    [SerializeField]
    private Color _characterGlowColor;
    [SerializeField]
    private float _characterGlowIntensity;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        SetEnvironmentColor();
        SetAmbienceLight();
        SetCharacterGlow();
        if (transform.position.x != _followTo.position.x)
        {
            transform.position = new Vector3(_followTo.position.x, transform.position.y, transform.position.z);
            // Debug.Log("Position changed");
        }
    }

    private void SetEnvironmentColor()
    {
        var dir = _directionalLight.GetComponent<Light>();
        if (!dir.color.Equals(_color)) dir.color = _color;
    }

    private void SetAmbienceLight()
    {
        var light = _spotLight.GetComponent<Light>();
        if (!light.color.Equals(_ambienceColor)) light.color = _ambienceColor;
        if (!light.range.Equals(_ambienceRange)) light.range = _ambienceRange;
        if (!light.intensity.Equals(_ambienceIntensity)) light.intensity = _ambienceIntensity;
    }

    private void SetCharacterGlow()
    {
        var light = _pointLight.GetComponent<Light>();
        if (!light.color.Equals(_characterGlowColor)) light.color = _characterGlowColor;
        if (!light.range.Equals(_characterGlowRange)) light.range = _characterGlowRange;
        if (!light.intensity.Equals(_characterGlowIntensity)) light.intensity = _characterGlowIntensity;
    }

    public void SetFollowTo(Transform element)
    {
        _followTo = element;
    }
}
