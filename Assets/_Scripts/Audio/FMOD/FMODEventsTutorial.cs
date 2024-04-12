using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsTutorial : MonoBehaviour
{
    public static FMODEventsTutorial Instance;

    [Header("Jump")]
    [SerializeField] public EventReference jumpSvart;

    [Header("Hit")]
    [SerializeField] public EventReference hitSvart;

    [Header("BreakableWall")]
    [SerializeField] public EventReference breakableWall;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um FMODEvents");
        }

        Instance = this;
    }
}
