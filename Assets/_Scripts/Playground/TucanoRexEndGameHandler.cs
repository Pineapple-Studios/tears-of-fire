using System;
using UnityEngine;

public class TucanoRexEndGameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _cvFeedback;
    [SerializeField]
    private GameObject[] _elementsToDeactive;
    [SerializeField]
    private GameObject[] _elementsToActive;

    private void OnEnable()
    {
        TucanoRexProps.onTucanoRexDead += OnTucanoRexDead;
    }

    private void OnDisable()
    {
        TucanoRexProps.onTucanoRexDead -= OnTucanoRexDead;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale == 0f) RoolBackToGame();
    }

    private void OnTucanoRexDead(GameObject obj)
    {
        Time.timeScale = 0f;
        _cvFeedback.SetActive(true);
        DeactiveElements();
    }

    private void DeactiveElements()
    {
        foreach (GameObject element in _elementsToDeactive)
        {
            element.SetActive(false);
        }
    }

    private void RoolBackToGame()
    {
        Time.timeScale = 1f;
        _cvFeedback.SetActive(false);
        ActiveElements();
        // Enable dash
        LevelDataManager.Instance.SetDashState(true);
    }

    private void ActiveElements()
    {
        foreach(GameObject element in _elementsToActive)
        {
            element.SetActive(true);
        }
    }
}
