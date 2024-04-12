using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um Audio Manager");
        }

        Instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

}
