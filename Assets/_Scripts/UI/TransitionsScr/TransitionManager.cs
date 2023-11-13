using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] GameObject _startingSceneTransition;
    [SerializeField] GameObject _endingSceneTransition;
    // Start is called before the first frame update
    private void Start()
    {
        _startingSceneTransition.SetActive(true);
        
    }

    private void DisableStartingSceneTransition()
    {
        _startingSceneTransition.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
