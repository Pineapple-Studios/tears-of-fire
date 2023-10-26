using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveGO : MonoBehaviour
{
    [SerializeField] public GameObject Go;

    public void ToggleGO()
    {
        Go.SetActive(!Go.activeSelf);
    }
}
