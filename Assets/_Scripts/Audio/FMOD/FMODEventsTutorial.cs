using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsTutorial : MonoBehaviour
{
    public static FMODEventsTutorial Instance;

    [Header("Jump")]
    [SerializeField] public EventReference jumpSvart;
    [SerializeField] public EventReference fallSvart;

    [Header("Hit")]
    [SerializeField] public EventReference hitSvart;

    [Header("BreakableWall")]
    [SerializeField] public EventReference breakableWall;

    [Header("LifeItem")]
    [SerializeField] public EventReference catchLifeItem;
    [SerializeField] public EventReference denyLifeItem;

    [Header("Gotinha")]
    [SerializeField] public EventReference leak;

    [Header("Death")]
    [SerializeField] public EventReference death;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um FMODEvents");
        }

        Instance = this;
    }
}
