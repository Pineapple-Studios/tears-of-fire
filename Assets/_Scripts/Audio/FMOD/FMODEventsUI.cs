using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsUI : MonoBehaviour
{
    public static FMODEventsUI Instance;

    [Header("MovementUI")]
    [SerializeField] public EventReference moveUI;  


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um FMODEvents");
        }

        Instance = this;
    }
}
