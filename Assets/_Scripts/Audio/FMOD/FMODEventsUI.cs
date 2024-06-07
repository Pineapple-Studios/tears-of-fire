using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsUI : MonoBehaviour
{
    public static FMODEventsUI Instance;

    [Header("SplashScreen")]
    [SerializeField] public EventReference splashScreen;
    [Space(10)]
    [Header("SplashScreen")]
    [SerializeField] public EventReference forBetterExp;
    [Space(10)]
    [Header("MovementUI")]
    [SerializeField] public EventReference moveUI;
    [Space(10)]
    [SerializeField] public EventReference clickUI;
    [Space(10)]
    [Header("Sliders")]
    [SerializeField] public EventReference sldMove;

    [Space(10)]
    [Header("Cutscenes")]
    [SerializeField] public EventReference voiceActing;
    [SerializeField] public EventReference introST;

    [SerializeField] public EventReference finalST;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um FMODEvents");
        }

        Instance = this;
    }
}
