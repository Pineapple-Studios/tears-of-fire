using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] string NextScreen;
    [Tooltip("Tempo para seguir para próxima tela")]
    [SerializeField] float TimeToGo = 3;

    [Header("Transition")]


    private float _timeCounter = 0;

    private void Start()
    {
        _timeCounter = 0;
    }

    void Update()
    {
        _timeCounter += Time.deltaTime;
        // Jump to next screen after 3sec
        if (_timeCounter > TimeToGo)
        {
            SceneManager.LoadScene(NextScreen);
        }
    }
}
