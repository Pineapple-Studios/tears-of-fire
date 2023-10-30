using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [HideInInspector] public InputActions controls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        controls = new InputActions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
